using System;
using System.Drawing;
using GameServer;

namespace NpcScriptEngine  {
	public class NpcDialog {

		public void Initialize(GameClient Client) {
			NpcDialogBuilder.Avatar(Client, 1);
			NpcDialogBuilder.Text(Client, "NPC ID: " + Client.ActiveNPC + " has not been implemented yet.");
			NpcDialogBuilder.Option(Client, 0xFF, "Close");
			NpcDialogBuilder.Finish(Client);				
		}
	}
}