﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NPCInfo
{
	// NPC 코드입니다.
	public string npcCode;

	// NPC 이름
	public string npcName;

	// NPC 대화시 표시될 문자열
	public string[] dialogMessages;
}


