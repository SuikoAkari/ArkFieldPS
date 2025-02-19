// Copyright (c) 2025, Shimizu Izumi. All rights reserved.

using ArkFieldPS.Network;
using ArkFieldPS.Protocol;

namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsSceneSubmitItem
    {
        [Server.Handler(CsMessageId.CsSceneSubmitItem)]
        public static void Handle(Player session, CsMessageId msgId, Packet packet)
        {
            CsSceneSubmitItem req = packet.DecodeBody<CsSceneSubmitItem>();
            
            session.Send(ScMessageId.ScSceneSubmitItem, new ScSceneSubmitItem
            {
                SubmitId = req.SubmitId,
                Ret = true
            });
        }
    }
}