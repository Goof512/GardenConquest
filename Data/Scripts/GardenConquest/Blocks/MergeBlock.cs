﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Common;
using Sandbox.Definitions;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Serializer;
using Sandbox.Engine;
using Sandbox.Game;
using Sandbox.ModAPI;
using Interfaces = Sandbox.ModAPI.Interfaces;
using InGame = Sandbox.ModAPI.Ingame;

namespace GardenConquest.Blocks {
	[MyEntityComponentDescriptor(typeof(MyObjectBuilder_MergeBlock))]
	class MergeBlock : MyGameLogicComponent {

		private IMyCubeGrid m_Grid = null;
		private InGame.IMyShipMergeBlock m_MergeBlock = null;

		private Logger m_Logger = null;

		public override void Init(MyObjectBuilder_EntityBase objectBuilder) {
			base.Init(objectBuilder);
			m_MergeBlock = Entity as InGame.IMyShipMergeBlock;
			m_Grid = m_MergeBlock.CubeGrid as IMyCubeGrid;

			m_Logger = new Logger(m_Grid.EntityId.ToString(), "MergeBlock");
			log("Attached to merge block", "Init");

			(m_MergeBlock as IMyShipMergeBlock).BeforeMerge += beforeMerge;
		}

		public override void Close() {
			log("Merge block closing", "Close");
			(m_MergeBlock as IMyShipMergeBlock).BeforeMerge -= beforeMerge;
		}

		private void beforeMerge() {
			log("Merge about to occur.  Marking grid.", "beforeMerge");

			GridEnforcer ge = m_Grid.Components.Get<MyGameLogicComponent>() as GridEnforcer;
			if (ge != null)
				ge.markForMerge();
			else
				log("GridEnforcer is null");
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false) {
			return Entity.GetObjectBuilder();
		}

		private void log(String message, String method = null, Logger.severity level = Logger.severity.DEBUG) {
			if (m_Logger != null)
				m_Logger.log(level, method, message);
		}
	}
}
