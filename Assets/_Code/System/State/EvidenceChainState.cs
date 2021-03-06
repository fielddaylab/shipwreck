﻿using BeauData;
using BeauUtil;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shipwreck {

	public interface IEvidenceChainState {
		StringHash32 Root();
		StringHash32 Next(StringHash32 current);
		void Lock();
		bool Contains(StringHash32 node);
		bool ContainsSet(IEnumerable<StringHash32> node);
		void Lift(StringHash32 node);
		void Drop(StringHash32 node, Vector2 position);
	}

	public sealed partial class GameMgr { // EvidenceChainState.cs
		private sealed partial class GameState { // EvidenceChainState.cs


			private class EvidenceChainState : IEvidenceChainState, ISerializedObject {

				private StringHash32 m_rootNode;
				private List<StringHash32> m_chain;
				private Vector2 m_danglePos;
				private bool m_isLocked;

				public EvidenceChainState() {
					// empty constructor for deserialization
				}
				public EvidenceChainState(StringHash32 root, Vector2 danglingPos) {
					m_rootNode = root;
					m_chain = new List<StringHash32>();
					m_danglePos = danglingPos;
					m_isLocked = false;
				}

				public StringHash32 Root() {
					return m_rootNode;
				}
				public StringHash32 Next(StringHash32 current) {
					if (current == m_rootNode) {
						if (m_chain.Count > 0) {
							return m_chain[0];
						} else {
							return StringHash32.Null;
						}
					} else {
						int index = m_chain.IndexOf(current);
						if (index == -1 || index + 1 >= m_chain.Count) {
							return StringHash32.Null;
						} else {
							return m_chain[index + 1];
						}
					}
				}

				public void Lock() {
					m_isLocked = true;
				}

				public bool Contains(StringHash32 node) {
					return node == m_rootNode || m_chain.Contains(node);
				}
				public bool ContainsSet(IEnumerable<StringHash32> set) {
					foreach (StringHash32 node in set) {
						if (!Contains(node)) {
							return false;
						}
					}
					return true;
				}

				/// <summary>
				/// Lifts the pin currently attached to the given node. If the root node is
				/// provided, the current dangling pin will be lifted.
				/// </summary>
				public void Lift(StringHash32 node) {
					if (m_isLocked) {
						throw new InvalidOperationException("Cannot Lift pins when the Chain is locked!");
					}
					if (!m_chain.Contains(node) && node != m_rootNode) {
						throw new InvalidOperationException(string.Format("Cannot Lift node `{0}' " +
							"because it is not part of this chain!", node.ToDebugString()
						));
					}
					while (m_chain.Count > 0 && m_chain[m_chain.Count-1] != node) {
						m_chain.RemoveAt(m_chain.Count - 1);
						// intentionally removing all nodes in the chain
						// (including the lifted one) until the desired
						// node is found or chain is empty
					}
					if (m_chain.Count > 0) {
						m_chain.RemoveAt(m_chain.Count - 1);
					}
				}

				/// <summary>
				/// Drops a pin at the end of the chain at the given node and moves 
				/// the dangling pin to danglePos. If the root node is provided, 
				/// no pin will be added. 
				/// </summary>
				public void Drop(StringHash32 node, Vector2 danglePos) {
					if (m_isLocked) {
						throw new InvalidOperationException("Cannot Drop pins when the Chain is locked!");
					}
					if (node != m_rootNode) {
						if (Contains(node)) {
							throw new InvalidOperationException("Cannot Drop " +
								"pins on nodes that are already within the chain."
							);
						}
						m_chain.Add(node);
					}
					m_danglePos = danglePos;
				}


				public void Serialize(Serializer ioSerializer) {

					ioSerializer.UInt32Proxy("root", ref m_rootNode);
					ioSerializer.UInt32ProxyArray("chain", ref m_chain);
					ioSerializer.Serialize("danglePos", ref m_danglePos);
					ioSerializer.Serialize("isLocked", ref m_isLocked);

				}

			}
		}
	}
}