using System;
using System.Drawing;
using GameServer;

namespace NpcScriptEngine  {
	public class NpcDialog {
		
		public void Process(GameClient Client, uint UID,  int OptionID, string Input) {
			switch(OptionID) {
				case 0:
					Client.Message("NPC " + UID + " is not implemented yet!", ChatType.Center, Color.Red);
				break;
			}
		}	
	}
}