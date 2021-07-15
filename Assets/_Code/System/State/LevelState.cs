﻿using BeauData;
using BeauUtil;
using System.Collections.Generic;
using UnityEngine;

namespace Shipwreck {

	public interface ILevelState {
		bool IsUnlocked { get; }
	}


	public sealed partial class GameMgr { // LevelState.cs

		private sealed partial class GameState { // LevelState.cs

			private class LevelState : ILevelState, ISerializedObject, ISerializedVersion {
				public ushort Version {
					get { return 1; }
				}

				public bool IsUnlocked {
					get { return m_isUnlocked; }
				}

				public IEnumerable<IEvidenceGroupState> Evidence {
					get {
						foreach (EvidenceGroupState evidence in m_evidence) {
							yield return evidence;
						}
					}
				}

				// serialized
				private bool m_isUnlocked = false;
				private List<EvidenceGroupState> m_evidence;
				private List<EvidenceChainState> m_chains;

				public LevelState() {
					m_evidence = new List<EvidenceGroupState>();
					m_chains = new List<EvidenceChainState>();
					UnlockEvidence("Main");
					UnlockEvidence("ShipCard");
				}

				public bool Unlock() {
					if (m_isUnlocked) {
						return false;
					} else {
						m_isUnlocked = true;
						return true;
					}
				}
				public bool UnlockEvidence(StringHash32 group) {
					if (IsEvidenceUnlocked(group)) {
						return false;
					} else {
						// todo: determine position
						m_evidence.Add(new EvidenceGroupState(group, Vector2.zero));
						return true;
					}
				}
				public bool IsEvidenceUnlocked(StringHash32 group) {
					return m_evidence.Find((item) => {
						return item.Identity == group;
					}) != null;
				}
				

				public void Serialize(Serializer ioSerializer) {
					ioSerializer.Serialize("isUnlocked", ref m_isUnlocked);
					ioSerializer.ObjectArray("evidence", ref m_evidence);
					ioSerializer.ObjectArray("connections", ref m_chains);
				}
			}
		}
	}
}