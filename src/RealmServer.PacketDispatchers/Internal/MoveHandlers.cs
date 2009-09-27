﻿using System;
using System.IO;
using Hazzik.Net;
using Hazzik.Objects;

namespace Hazzik.RealmServer.PacketDispatchers.Internal {
	[PacketHandlerClass(WMSG.MSG_MOVE_START_FORWARD)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_BACKWARD)]
	[PacketHandlerClass(WMSG.MSG_MOVE_STOP)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_STRAFE_LEFT)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_STRAFE_RIGHT)]
	[PacketHandlerClass(WMSG.MSG_MOVE_STOP_STRAFE)]
	[PacketHandlerClass(WMSG.MSG_MOVE_JUMP)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_TURN_LEFT)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_TURN_RIGHT)]
	[PacketHandlerClass(WMSG.MSG_MOVE_STOP_TURN)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_PITCH_UP)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_PITCH_DOWN)]
	[PacketHandlerClass(WMSG.MSG_MOVE_STOP_PITCH)]
	[PacketHandlerClass(WMSG.MSG_MOVE_SET_RUN_MODE)]
	[PacketHandlerClass(WMSG.MSG_MOVE_SET_WALK_MODE)]
	[PacketHandlerClass(WMSG.MSG_MOVE_START_SWIM)]
	[PacketHandlerClass(WMSG.MSG_MOVE_STOP_SWIM)]
	[PacketHandlerClass(WMSG.MSG_MOVE_SET_FACING)]
	[PacketHandlerClass(WMSG.MSG_MOVE_SET_PITCH)]
	[PacketHandlerClass(WMSG.MSG_MOVE_ROOT)]
	[PacketHandlerClass(WMSG.MSG_MOVE_UNROOT)]
	[PacketHandlerClass(WMSG.MSG_MOVE_HEARTBEAT)]
	[PacketHandlerClass(WMSG.MSG_MOVE_KNOCK_BACK)]
	[PacketHandlerClass(WMSG.MSG_MOVE_HOVER)]
	[PacketHandlerClass(WMSG.MSG_MOVE_FALL_LAND)]
	internal class MoveHandlers : IPacketDispatcher {
		#region IPacketDispatcher Members

		public void Dispatch(ISession client, IPacket packet) {
			Player me = client.Player;
			Session.SendNearExceptMe(me, GetMoveResponce(packet));
			BinaryReader reader = packet.CreateReader();
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			ulong guid = reader.ReadPackGuid();
			if(guid != me.Guid) {
				throw new Exception();
			}
			me.MovementInfo.Read(reader);
		}

		#endregion

		private static IPacket GetMoveResponce(IPacket packet) {
			BinaryReader reader = packet.CreateReader();
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			byte[] bytes = reader.ReadBytes(packet.Size);

			IPacket responce = WorldPacketFactory.Create((WMSG)packet.Code);
			BinaryWriter w = responce.CreateWriter();
			w.Write(bytes);
			return responce;
		}
	}
}