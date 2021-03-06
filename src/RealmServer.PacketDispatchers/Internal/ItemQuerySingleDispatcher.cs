using System;
using System.IO;
using Hazzik.Data;
using Hazzik.Items;
using Hazzik.Net;

namespace Hazzik.RealmServer.PacketDispatchers.Internal {
	[WorldPacketHandler(WMSG.CMSG_ITEM_QUERY_SINGLE)]
	internal class ItemQuerySingleDispatcher : IPacketDispatcher {
		#region IPacketDispatcher Members

		public void Dispatch(ISession session, IPacket packet) {
			BinaryReader r = packet.CreateReader();
			uint itemId = r.ReadUInt32();
			ItemTemplate template = IoC.Resolve<IItemTemplateRepository>().FindById(itemId);
			if(template != null) {
				session.SendItemQuerySingleResponse(template);
			}
		}

		#endregion
	}
}