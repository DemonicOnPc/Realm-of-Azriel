﻿using UnityEngine;
using UnityEngine.Profiling;

namespace MultiplayerARPG
{
    public abstract class UIBaseGameEntity<T> : UISelectionEntry<T>
        where T : BaseGameEntity
    {
        public enum Visibility
        {
            VisibleWhenSelected,
            VisibleWhenNearby,
            AlwaysVisible,
        }

        [Header("Base Game Entity - String Formats")]
        [Tooltip("Format => {0} = {Title}")]
        public UILocaleKeySetting formatKeyTitle = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_SIMPLE);
        [Tooltip("Format => {0} = {TitleB}")]
        public UILocaleKeySetting formatKeyTitleB = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_SIMPLE);

        [Header("Base Game Entity - UI Elements")]
        public TextWrapper uiTextTitle;
        public TextWrapper uiTextTitleB;

        [Header("Visible Options")]
        public Visibility visibility;
        public float visibleDistance = 30f;

        private float lastShowTime;
        private BasePlayerCharacterEntity tempOwningCharacter;
        private BaseGameEntity tempTargetEntity;

        private Canvas cacheCanvas;
        public Canvas CacheCanvas
        {
            get
            {
                if (cacheCanvas == null)
                    cacheCanvas = GetComponent<Canvas>();
                return cacheCanvas;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            CacheCanvas.enabled = false;
        }

        protected override void Update()
        {
            base.Update();

            if (!CacheCanvas.enabled)
                return;

            string tempTitle;
            if (uiTextTitle != null)
            {
                tempTitle = Data == null ? string.Empty : Data.Title;
                uiTextTitle.text = string.Format(
                    LanguageManager.GetText(formatKeyTitle),
                    tempTitle);
                uiTextTitle.gameObject.SetActive(!string.IsNullOrEmpty(tempTitle));
            }

            if (uiTextTitleB != null)
            {
                tempTitle = Data == null ? string.Empty : Data.TitleB;
                uiTextTitleB.text = string.Format(
                    LanguageManager.GetText(formatKeyTitleB),
                    tempTitle);
                uiTextTitleB.gameObject.SetActive(!string.IsNullOrEmpty(tempTitle));
            }
        }

        protected virtual bool ValidateToUpdateUI()
        {
            return Data != null &&
                BasePlayerCharacterController.OwningCharacter != null;
        }

        protected override void UpdateUI()
        {
            if (!ValidateToUpdateUI())
            {
                CacheCanvas.enabled = false;
                return;
            }

            Profiler.BeginSample("UIBaseGameEntity - Update UI");
            tempOwningCharacter = BasePlayerCharacterController.OwningCharacter;
            if (tempOwningCharacter == Data)
            {
                // Always show the UI when character is owning character
                CacheCanvas.enabled = true;
            }
            else
            {
                switch (visibility)
                {
                    case Visibility.VisibleWhenSelected:
                        tempTargetEntity = BasePlayerCharacterController.Singleton.SelectedEntity;
                        CacheCanvas.enabled = tempTargetEntity != null &&
                            tempTargetEntity.ObjectId == Data.ObjectId &&
                            Vector3.Distance(tempOwningCharacter.CacheTransform.position, Data.CacheTransform.position) <= visibleDistance;
                        break;
                    case Visibility.VisibleWhenNearby:
                        CacheCanvas.enabled = Vector3.Distance(tempOwningCharacter.CacheTransform.position, Data.CacheTransform.position) <= visibleDistance;
                        break;
                    case Visibility.AlwaysVisible:
                        CacheCanvas.enabled = true;
                        break;
                }
            }
            Profiler.EndSample();
        }

        protected override void UpdateData() { }
    }

    public class UIBaseGameEntity : UIBaseGameEntity<BaseGameEntity> { }
}
