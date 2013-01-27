using System;
using System.Drawing;
using GameServer;

namespace NpcScriptEngine  {
	public class NpcDialog {
		
		public void Process(GameClient Client, uint UID, int OptionID, string Input) {
			switch(OptionID) {
				case 0:
					Client.Teleport(1003, 50, 50);
				break;
			}
		}	
	}
}