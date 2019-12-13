﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class StorageCharacterItemSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        StorageCharacterItem data = (StorageCharacterItem)obj;
        info.AddValue("storageType", (byte)data.storageType);
        info.AddValue("storageId", data.storageOwnerId);
        info.AddValue("dataId", data.characterItem.dataId);
        info.AddValue("level", data.characterItem.level);
        info.AddValue("amount", data.characterItem.amount);
        info.AddValue("durability", data.characterItem.durability);
        info.AddValue("exp", data.characterItem.exp);
        info.AddValue("lockRemainsDuration", data.characterItem.lockRemainsDuration);
        info.AddValue("ammo", data.characterItem.ammo);
        info.AddValue("sockets", data.characterItem.sockets);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        StorageCharacterItem data = (StorageCharacterItem)obj;
        data.storageType = (StorageType)info.GetByte("storageType");
        data.storageOwnerId = info.GetString("storageId");
        CharacterItem characterItem = new CharacterItem();
        characterItem.dataId = info.GetInt32("dataId");
        characterItem.level = info.GetInt16("level");
        characterItem.amount = info.GetInt16("amount");
        characterItem.durability = info.GetSingle("durability");
        characterItem.exp = info.GetInt32("exp");
        characterItem.lockRemainsDuration = info.GetSingle("lockRemainsDuration");
        characterItem.ammo = info.GetInt16("ammo");
        try
        {
            characterItem.sockets = (List<int>)info.GetValue("sockets", typeof(List<int>));
        }
        catch { }
        data.characterItem = characterItem;
        obj = data;
        return obj;
    }
}
