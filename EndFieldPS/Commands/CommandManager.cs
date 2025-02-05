namespace EndFieldPS.Commands
{
    using Pastel;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Collections.Immutable;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Net.Sockets;
    using EndFieldPS.Protocol;
    using EndFieldPS.Network;

    public static class CommandManager
    {
        private static List<Type> s_handlerTypes = new List<Type>();
        public static ImmutableDictionary<string, (Server.CommandAttribute, Server.CommandAttribute.HandlerDelegate)> s_notifyReqGroup;
        public static string targetId;
        public static void Init()
        {
            var handlers = ImmutableDictionary.CreateBuilder<string, (Server.CommandAttribute, Server.CommandAttribute.HandlerDelegate)>();

            foreach (var type in s_handlerTypes)
            {
                foreach (var method in type.GetMethods())
                {
                    var attribute = method.GetCustomAttribute<Server.CommandAttribute> ();
                    if (attribute == null)
                        continue;

                    var parameterInfo = method.GetParameters();
                    var senderParameter = Expression.Parameter(typeof(Player));
                    var commandParameter = Expression.Parameter(typeof(string));
                    var argsParameter = Expression.Parameter(typeof(string[]));
                    var targetParameter = Expression.Parameter(typeof(Player));

                    var call = Expression.Call(method, 
                        Expression.Convert(senderParameter, parameterInfo[0].ParameterType),
                        Expression.Convert(commandParameter, parameterInfo[1].ParameterType),
                        Expression.Convert(argsParameter, parameterInfo[2].ParameterType),
                        Expression.Convert(targetParameter, parameterInfo[3].ParameterType));

                    var lambda = Expression.Lambda<Server.CommandAttribute.HandlerDelegate>(call,senderParameter, commandParameter, argsParameter,targetParameter);

                    if (!handlers.TryGetKey(attribute.command, out _))
                        handlers.Add(attribute.command, (attribute, lambda.Compile()));
                }
            }

            s_notifyReqGroup = handlers.ToImmutable();
        }

        public static void Notify(Player sender,string cmd, string[] args,Player target)
        {
            if (s_notifyReqGroup.TryGetValue(cmd, out var handler))
            {
                if (handler.Item1.requiredTarget)
                {
                    if (target != null)
                    {
                        handler.Item2.Invoke(sender,cmd, args, target);
                    }
                    else
                    {
                        SendMessage(sender,"This command require a target player, set one with /target (uid)");
                    }
                    
                }
                else
                {
                    handler.Item2.Invoke(sender, cmd, args, target);
                }
                
            }
            else
            {
                SendMessage(sender, $"Command not found");
            }
        }

        public static void AddReqGroupHandler(Type type)
        {
            s_handlerTypes.Add(type);
        }

        
        public static void SendMessage(Player sender, string msg)
        {
            if (sender == null)
            {
                Logger.Print(msg);
            }
            else
            {
                //For sending to player command prompt page made by Xannix
                sender.temporanyChatMessages.Add(msg);
                
            }
        }
    }
}
