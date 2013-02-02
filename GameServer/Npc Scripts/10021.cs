using System;
using System.Drawing;
using GameServer;

namespace NpcScriptEngine  {
	public class NpcDialog {

		public void Initialize(GameClient Client) {
			NpcDialogBuilder.Avatar(Client, 2);
			NpcDialogBuilder.Text(Client, "Do you wish to be teleported to arena?");
			NpcDialogBuilder.Option(Client, 0x00, "Yes, please");
			NpcDialogBuilder.Finish(Client);				
		}
		public void Process(GameClient Client, byte OptionID, string Input) {
			switch(OptionID) {
				case 0:
					Client.Teleport(1005, 51, 70);
				break;
			}
		}
	}
}