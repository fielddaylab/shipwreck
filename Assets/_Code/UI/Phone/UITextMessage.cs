﻿using PotatoLocalization;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using BeauRoutine;
using Leaf.Defaults;
using BeauUtil.Tags;
using System;
using BeauUtil;

namespace Shipwreck {

	public class UITextMessage : UIDialogueBase {

		[SerializeField]
		private RectTransform m_content = null;
		[SerializeField]
		private LayoutGroup m_layout = null;
		[SerializeField]
		private LocalizedTextUGUI m_conversationPartner = null;
		[SerializeField]
		private ScrollRect m_scrollRect = null;
		[SerializeField]
		private Button m_continueButton = null;
		[Header("Prefabs")]
		[SerializeField]
		private TextMessageText m_textPrefab = null;
		[SerializeField]
		private TextMessageImage m_imagePrefab = null;

		[NonSerialized]
		private CharacterData m_currentCharacter = null;

		#region UIBase

		protected override void OnShowStart() {
			base.OnShowStart();
			UIMgr.Open<UIPhone>();
			UIMgr.Close<UIContacts>();
			ClearContent();
		}

		protected override void OnHideCompleted() {
			base.OnHideCompleted();
			ClearContent();
		}

		protected override IEnumerator HideRoutine() {
			yield break;
		}

		protected override IEnumerator ShowRoutine() {
			yield break;
		}

		#endregion // UIBase

		#region Dialog

		protected override void AssignPartner(CharacterData character) {
			m_conversationPartner.Key = character.DisplayName;
		}
		
		protected override void OnSetSpeaker(CharacterData speaker) {
			m_currentCharacter = speaker;
		}

		protected override void OnPrepareLine(TagString inString) {
			// do nothing
		}

		protected override IEnumerator OnShowImage(Sprite image) {
			TextMessageImage obj = Instantiate(m_imagePrefab, m_content);
			obj.Populate(m_currentCharacter, image);
			m_layout.ForceRebuild();
			yield return m_scrollRect.NormalizedPosTo(0f, 0.1f, Axis.Y);
			yield return 0.15f;

			yield return CompleteLine();
		}

		protected override IEnumerator OnHideImage() {
			return null;
		}

		public override IEnumerator TypeLine(TagString inString, TagTextData inType) {
			TextMessageText obj = Instantiate(m_textPrefab, m_content);
			obj.Populate(m_currentCharacter, inString.RichText);
			m_layout.ForceRebuild();
			yield return m_scrollRect.NormalizedPosTo(0f, 0.1f, Axis.Y);
			yield return 0.15f;
		}

		public override IEnumerator CompleteLine() {
			yield return m_continueButton.onClick.WaitForInvoke();
		}

		#endregion // Dialog

		private void ClearContent() {
			for (int ix = m_content.childCount - 1; ix >= 0; ix--) {
				Destroy(m_content.GetChild(ix).gameObject);
			}
		}
	}

}