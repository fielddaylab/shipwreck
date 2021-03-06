﻿using BeauUtil;
using BeauUtil.Tags;
using Leaf.Defaults;
using System.Collections;
using UnityEngine;

namespace Shipwreck {


	public abstract class UIDialogueBase : UIBase, ITextDisplayer {

		private TagStringEventHandler m_tagEvents;

		protected string CachedText { get; private set; }

		private void Awake() {
			if (m_tagEvents != null) {
				return;
			}
			m_tagEvents = new TagStringEventHandler();
			m_tagEvents.Register(ScriptEvents.Dialog.Target, HandleSetTarget);
			m_tagEvents.Register(ScriptEvents.Dialog.Image, HandleShowImage);
			m_tagEvents.Register(ScriptEvents.Dialog.HideImage, HandleHideImage);
		}

		private void HandleSetTarget(TagEventData inEvent, object inContext) {
			OnSetSpeaker(GameDb.GetCharacterData(inEvent.GetStringHash()));
		}
		private IEnumerator HandleShowImage(TagEventData inEvent, object inContext) {
			return OnShowImage(GameDb.GetImageData(inEvent.GetStringHash()));
		}
		private IEnumerator HandleHideImage() {
			return OnHideImage();
		}

		public virtual void PrepareNode(ScriptNode node) {
			AssignPartner(GameDb.GetCharacterData(node.ContactId));
		}

		public TagStringEventHandler PrepareLine(TagString inString, TagStringEventHandler inBaseHandler) {
			CachedText = inString.RichText;
			OnPrepareLine(inString);
			m_tagEvents.Base = inBaseHandler;
			return m_tagEvents;
		}

		public abstract IEnumerator TypeLine(TagString inString, TagTextData inType);
		public abstract IEnumerator CompleteLine();

		protected abstract void AssignPartner(CharacterData character);
		protected abstract void OnPrepareLine(TagString inString);
		protected abstract void OnSetSpeaker(CharacterData speaker);
		protected abstract IEnumerator OnShowImage(Sprite image);
		protected abstract IEnumerator OnHideImage();
	}

}