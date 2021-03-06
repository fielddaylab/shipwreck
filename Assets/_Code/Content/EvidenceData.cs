﻿using UnityEngine;
using PotatoLocalization;
using BeauUtil;
using System.Collections.Generic;

namespace Shipwreck {

	[CreateAssetMenu(fileName = "NewEvidenceData", menuName = "Shipwrecks/Evidence")]
	public class EvidenceData : ScriptableObject {

		public StringHash32 GroupID {
			get { return m_groupID; }
		}

		public EvidenceGroup NodeGroup {
			get {
				if (m_isLocalized) {
					if (LocalizationMgr.CurrentLanguage == new LanguageCode("en")) {
						return m_englishPrefab;
					} else {
						return m_spanishPrefab;
					}
				} else {
					return m_englishPrefab;
				}
			}
		}
		public IEnumerable<StringHash32> RootNodes {
			get {
				foreach (SerializedHash32 node in m_rootNodes) {
					yield return node;
				}
			}
		}

		

		[SerializeField]
		private SerializedHash32 m_groupID;
		[SerializeField, Tooltip("Does this use a different prefab based on language?")]
		private bool m_isLocalized = false;
		[SerializeField]
		private EvidenceGroup m_englishPrefab = null;
		[SerializeField]
		private EvidenceGroup m_spanishPrefab = null;
		[SerializeField]
		private SerializedHash32[] m_rootNodes = null;

	}

}


