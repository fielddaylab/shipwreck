﻿using BeauUtil;
using BeauUtil.Debugger;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shipwreck {

	public class GameDb : Singleton<GameDb> {

		[SerializeField]
		private CharacterData[] m_characters;
		[SerializeField]
		private EvidenceData[] m_evidenceData;
		[SerializeField]
		private Sprite[] m_images;

		[NonSerialized]
		private Dictionary<StringHash32, CharacterData> m_characterMap;
		[NonSerialized]
		private Dictionary<StringHash32, EvidenceData> m_evidenceMap;		
		[NonSerialized]
		private Dictionary<StringHash32, Sprite> m_imageMap;
		public static CharacterData GetCharacterData(StringHash32 hash) {
			// initialize the map if it does not exist
			if (I.m_characterMap == null) {
				I.m_characterMap = new Dictionary<StringHash32, CharacterData>();
				foreach (CharacterData data in I.m_characters) {
					I.m_characterMap.Add(data.Hash, data);
				}
			}
			// find the tag within the map
			if (I.m_characterMap.ContainsKey(hash)) {
				return I.m_characterMap[hash];
			} else {
				throw new KeyNotFoundException(string.Format("No Character " +
					"with tag `{0}' is in the database", hash
				));
			}
		}
		public static Sprite GetImageData(StringHash32 hash) {
			if (I.m_imageMap == null) {
				I.m_imageMap = new Dictionary<StringHash32, Sprite>();
				foreach(Sprite sprite in I.m_images) {
					I.m_imageMap.Add(sprite.name, sprite);
				}
			}
			// find the tag within the map
			if (I.m_imageMap.ContainsKey(hash)) {
				return I.m_imageMap[hash];
			} else {
				throw new KeyNotFoundException(Log.Format("No " +
					"Image with id `{0}' is in the database", hash
				));
			}
		}

		public static EvidenceGroup GetEvidenceGroup(StringHash32 groupID) {
			// initialize the map if it does not exist
			if (I.m_evidenceMap == null) {
				I.m_evidenceMap = new Dictionary<StringHash32, EvidenceData>();
				foreach (EvidenceData data in I.m_evidenceData) {
					I.m_evidenceMap.Add(data.GroupID, data);
				}
			}
			if (I.m_evidenceMap.ContainsKey(groupID)) {
				return I.m_evidenceMap[groupID].NodeGroup;
			} else {
				throw new KeyNotFoundException(string.Format("No Evidence " +
					"with groupID `{0}' is in the database", groupID
				));
			}
		}
		public static EvidenceData GetEvidenceData(StringHash32 groupID) {
			// initialize the map if it does not exist
			if (I.m_evidenceMap == null) {
				I.m_evidenceMap = new Dictionary<StringHash32, EvidenceData>();
				foreach (EvidenceData data in I.m_evidenceData) {
					I.m_evidenceMap.Add(data.GroupID, data);
				}
			}
			if (I.m_evidenceMap.ContainsKey(groupID)) {
				return I.m_evidenceMap[groupID];
			} else {
				throw new KeyNotFoundException(string.Format("No Evidence " +
					"with groupID `{0}' is in the database", groupID
				));
			}
		}


	}


}
