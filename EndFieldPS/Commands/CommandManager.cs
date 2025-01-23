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
        private static ImmutableDictionary<string, (Server.CommandAttribute, Server.CommandAttribute.HandlerDelegate)> s_notifyReqGroup;

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

                    var commandParameter = Expression.Parameter(typeof(string));
                    var argsParameter = Expression.Parameter(typeof(string[]));
                   

                    var call = Expression.Call(method,
                        Expression.Convert(commandParameter, parameterInfo[0].ParameterType),
                        Expression.Convert(argsParameter, parameterInfo[1].ParameterType));

                    var lambda = Expression.Lambda<Server.CommandAttribute.HandlerDelegate>(call, commandParameter, argsParameter);

                    if (!handlers.TryGetKey(attribute.command, out _))
                        handlers.Add(attribute.command, (attribute, lambda.Compile()));
                }
            }

            s_notifyReqGroup = handlers.ToImmutable();
        }

        public static void Notify(string cmd, string[] args)
        {
            if (s_notifyReqGroup.TryGetValue(cmd, out var handler))
            {
                handler.Item2.Invoke(cmd,args);
            }
            else
            {
               Logger.Print($"Command not found");
            }
        }

        public static void AddReqGroupHandler(Type type)
        {
            s_handlerTypes.Add(type);
        }
    }
}
