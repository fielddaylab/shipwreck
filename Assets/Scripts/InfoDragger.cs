﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProtoCP;
using UnityEngine.EventSystems;

public class InfoDragger : MonoBehaviour
{
    public GameObject[] documents;
    public string[] documentNames;
    public TextMeshProUGUI[] targets;
    public TextMeshProUGUI[] sourceLabels;
    public GameObject infoChunkTemplate;

    private GameObject draggingObject;
    private ScrollRect draggedFromScroll;
    private Vector3 dragStartMouse;
    private Vector3 dragStartInfo;
    private TMP_LinkInfo linkInfo;
    private string documentName;

    // Start is called before the first frame update
    void Start()
    {
        int documentIndex = 0;
        foreach (var document in documents)
        {
            int thisDocumentIndex = documentIndex;
            foreach (var infoPiece in document.GetComponentsInChildren<TextMeshProUGUI>())
            {
                PointerListener pointer = infoPiece.GetComponent<PointerListener>();
                if (pointer)
                {
                    pointer.onPointerDown.AddListener((pdata) =>
                    {
                        int linkIndex = TMP_TextUtilities.FindIntersectingLink(infoPiece, pdata.position, null);
                        if (linkIndex != -1)
                        {
                            linkInfo = infoPiece.textInfo.linkInfo[linkIndex];

                            draggingObject = Instantiate(infoChunkTemplate, transform, true);
                            RectTransform draggingRect = draggingObject.GetComponent<RectTransform>();
                            draggingRect.anchorMin = new Vector2(0f, 1f);
                            draggingRect.anchorMax = new Vector2(0f, 1f);
                            draggingRect.pivot = new Vector2(0f, 0f);
                            draggingRect.position = Input.mousePosition;
                            TextMeshProUGUI draggingText = draggingObject.GetComponentInChildren<TextMeshProUGUI>();
                            draggingText.color = Color.black;
                            draggingText.text = linkInfo.GetLinkText();
                            draggedFromScroll = infoPiece.GetComponentInParent<ScrollRect>();
                            if (draggedFromScroll)
                            {
                                draggedFromScroll.enabled = false;
                            }
                            dragStartMouse = Input.mousePosition;
                            dragStartInfo = draggingRect.position;
                            draggingObject.SetActive(true);
                            documentName = documentNames[thisDocumentIndex];
                        }
                    });
                    pointer.onPointerUp.AddListener(ReleaseDrag);
                }
            }
            documentIndex++;
        }

        foreach (var sourceLabel in sourceLabels)
        {
            sourceLabel.text = "";
        }
    }

    void ReleaseDrag(PointerEventData pdata)
    {
        if (draggedFromScroll)
        {
            draggedFromScroll.enabled = true;
        }

        int targetIndex = 0;
        foreach (var target in targets)
        {
            Vector2 localMouse = target.rectTransform.InverseTransformPoint(Input.mousePosition);
            if (target.rectTransform.rect.Contains(localMouse))
            {
                target.text = draggingObject.GetComponentInChildren<TextMeshProUGUI>().text;
                sourceLabels[targetIndex].text = "From: " + documentName;
                break;
            }
            targetIndex++;
        }

        if (draggingObject)
        {
            Destroy(draggingObject.gameObject);
        }
        draggingObject = null;
        draggedFromScroll = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (draggingObject)
        {
            draggingObject.GetComponentInChildren<RectTransform>().position = dragStartInfo + (Input.mousePosition - dragStartMouse);
        }
        foreach (var target in targets)
        {
            Image targetBG = target.GetComponentInParent<Image>();
            if (targetBG)
            {
                if (draggingObject)
                {
                    Vector2 localMouse = target.rectTransform.InverseTransformPoint(Input.mousePosition);
                    targetBG.color = target.rectTransform.rect.Contains(localMouse) ? Color.gray : Color.white;
                }
                else
                {
                    targetBG.color = Color.white;
                }
            }
        }
    }
}