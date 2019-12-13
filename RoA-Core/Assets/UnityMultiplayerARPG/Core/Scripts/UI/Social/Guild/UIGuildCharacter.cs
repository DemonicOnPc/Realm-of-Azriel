﻿using UnityEngine;

namespace MultiplayerARPG
{
    public class UIGuildCharacter : UISocialCharacter
    {
        [Header("String Formats")]
        [Tooltip("Format => {0} = {Role Name}")]
        public UILocaleKeySetting formatKeyGuildRole = new UILocaleKeySetting(UIFormatKeys.UI_FORMAT_SIMPLE);
        
        [Header("UI Elements")]
        public TextWrapper uiGuildRole;

        public byte GuildRole { get; private set; }

        public void Setup(UISocialCharacterData data, byte guildRole, GuildRoleData guildRoleData)
        {
            Data = data;
            GuildRole = guildRole;

            if (uiGuildRole != null)
                uiGuildRole.text = string.Format(LanguageManager.GetText(formatKeyGuildRole), guildRoleData.roleName);
        }
    }
}
