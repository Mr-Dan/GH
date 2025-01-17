using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;


public class SizeObject
{

    public SizeObject(GameObject obj, float xWidth, float yHeight, float zLength)
    {
        this.obj = obj;
        this.xWidth = xWidth;
        this.yHeight = yHeight;
        this.zLength = zLength;
    }

    public (float, float, float) GetSizeXYZ()
    {
        return (xWidth, yHeight, zLength);
    }

    public GameObject obj;
    private float xWidth = 0;
    private float yHeight = 0;
    private float zLength = 0;
}

public class Generator : MonoBehaviour
{


    // tiles and xyz numbers
    private float xWidth = 0;
    private float yHeight = 0;
    private float zLength = 0;
    [Min(0)]
    public int Floor;
    [Min(0)]
    public int Entranceway;
    // [Min(0)]
    //public int zNumberWidth;


    public GameObject[] foundationObjects;
    public GameObject[] foundationMainObjects;
    public GameObject[] windowInWallsObjects;
    public GameObject[] sideWallObjects;
    public GameObject[] entrancewayObjects;
    public GameObject[] windowInWallLastObjects;
    public GameObject[] roofObjects;
    public GameObject[] roofSideWallObjects;


    public List<SizeObject> SizeObject;


    public void SetSizeObject()
    {
        SizeObject = new List<SizeObject>();
        SizeObject.Add(new SizeObject(foundationObjects[0], 3.4F, 1.5F, 5F));
        SizeObject.Add(new SizeObject(foundationMainObjects[0], 3.2F, 1.5F, 5F));
        SizeObject.Add(new SizeObject(windowInWallsObjects[0], 3.2F, 2.5F, 0.28F));
        SizeObject.Add(new SizeObject(sideWallObjects[0], 0.28F, 2.5F, 5F));
        SizeObject.Add(new SizeObject(entrancewayObjects[0], 3.2F, 2.5F, 2.1F));
        SizeObject.Add(new SizeObject(windowInWallLastObjects[0], 3.2F, 1.5F, 0.28F));
        SizeObject.Add(new SizeObject(roofObjects[0], 3.2F, 0.28F, 5.2F));
        SizeObject.Add(new SizeObject(roofSideWallObjects[0], 0.28F, 0.5F, 5F));
    }
    public void makeBuilding()
    {
        #region Начальные установки
        int Section = 0;
        float lastXTile = 0, lastYTile = 0, lastZTile = 0;
        int countEntranceBlock = 0;

        GameObject Parent = new GameObject("EntranceBlock");
        Parent.transform.parent = transform;

        int Lenght = Entranceway * 5;
        int Height = Floor;
        #endregion
        for (var i = 0; i <= Lenght - 1; i++)//Блоки
        {
            #region параметры
            float UpBackFront = 0.5F;
            float OffСenterFront = 7.64F;
            float OffСenterBack = -2.08F;
            float OffRight = -1.88F;
            float OffLeft = 1.7F;
            float RoofDown = 1.12F;
            lastYTile = 0;
            lastYTile += UpBackFront;
            #endregion
            for (var j = 0; j <= Height + 1; j++)//Этажи
            {
                #region Формирование группы подъезда
                if (countEntranceBlock == 6)
                {
                    Parent = new GameObject("EntranceBlock");
                    Parent.transform.parent = transform;
                    countEntranceBlock = 0;
                }
                #endregion
                if (j == 0)
                {
                    #region Фундаменты
                    if (i == 0) // Первая боковая сторона
                    {
                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == foundationObjects[0]).GetSizeXYZ();
                        GameObject foundationFront = Instantiate(foundationObjects[0], new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        foundationFront.transform.parent = Parent.transform;

                        GameObject foundationBack = Instantiate(foundationObjects[0], new Vector3(0, 0, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        foundationBack.transform.parent = Parent.transform;

                        lastYTile += yHeight;
                        lastXTile = 0.1F;
                        Section++;
                    }
                    else if (i == Lenght - 1) // Последная боковая сторона
                    {
                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == foundationObjects[0]).GetSizeXYZ();
                        GameObject foundationFront = Instantiate(foundationObjects[0], new Vector3(lastXTile + 0.1F, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        foundationFront.transform.parent = Parent.transform;

                        GameObject foundationBack = Instantiate(foundationObjects[0], new Vector3(lastXTile + 0.1F, 0, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        foundationBack.transform.parent = Parent.transform;

                        lastYTile += yHeight;
                    }
                    else
                    {
                        if (Section == 2) // Условия для формирование подъезда
                        {
                            Section -= 4;

                            (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == entrancewayObjects[0]).GetSizeXYZ();
                            GameObject entranceway = Instantiate(entrancewayObjects[0], new Vector3(lastXTile, 0, zLength + 5.26F), Quaternion.Euler(new Vector3(0, 0, 0)));
                            entranceway.transform.parent = Parent.transform;

                            lastYTile += yHeight;
                            //(xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == foundationMainObjects[0]).GetSizeXYZ();
                            //GameObject cornerObject1 = Instantiate(foundationObjects[0], new Vector3((float)(lastXTile + xWidth), (float)(0), (float)(zLength)), Quaternion.Euler(new Vector3(0, 0, 0)));
                            //cornerObject1.transform.parent = transform;

                            GameObject foundationBack = Instantiate(foundationMainObjects[0], new Vector3(lastXTile, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                            foundationBack.transform.parent = Parent.transform   ;

                        }
                        else // Фундаменты
                        {
                            (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == foundationMainObjects[0]).GetSizeXYZ();
                            GameObject foundationFront = Instantiate(foundationMainObjects[0], new Vector3(lastXTile, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                            foundationFront.transform.parent = Parent.transform;

                            GameObject foundationBack = Instantiate(foundationMainObjects[0], new Vector3(lastXTile, 0, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                            foundationBack.transform.parent = Parent.transform;

                            lastYTile += yHeight;
                            Section++;
                        }
                    }
                    #endregion
                }
                else if (j == Height + 1)
                {
                     #region Крыша
                    if (i == 0)
                    {
                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == roofSideWallObjects[0]).GetSizeXYZ();
                        GameObject roofSideWallFront = Instantiate(roofSideWallObjects[0], new Vector3(OffRight + xWidth + 0.04F, lastYTile - RoofDown + 0.26F, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofSideWallFront.transform.parent = Parent.transform;

                        GameObject roofSideWallBack = Instantiate(roofSideWallObjects[0], new Vector3(OffRight + xWidth + 0.04F, lastYTile - RoofDown + 0.26F, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofSideWallBack.transform.parent = Parent.transform;

                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == roofObjects[0]).GetSizeXYZ();
                        GameObject roofObjectsFront = Instantiate(roofObjects[0], new Vector3(0.1F, lastYTile - RoofDown, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofObjectsFront.transform.parent = Parent.transform;

                        GameObject roofObjectsBack = Instantiate(roofObjects[0], new Vector3(0.1F, lastYTile - RoofDown, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofObjectsBack.transform.parent = Parent.transform;
                    }
                    else if (i == Lenght - 1)
                    {

                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == roofSideWallObjects[0]).GetSizeXYZ();
                        GameObject roofSideWallFront = Instantiate(roofSideWallObjects[0], new Vector3(lastXTile + 1.66F, lastYTile - RoofDown + 0.26F, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofSideWallFront.transform.parent = Parent.transform;

                        GameObject roofSideWallBack = Instantiate(roofSideWallObjects[0], new Vector3(lastXTile + 1.66F, lastYTile - RoofDown + 0.26F, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofSideWallBack.transform.parent = Parent.transform;

                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == roofObjects[0]).GetSizeXYZ();
                        GameObject roofObjectsFront = Instantiate(roofObjects[0], new Vector3(lastXTile- 0.1F, lastYTile - RoofDown, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofObjectsFront.transform.parent = Parent.transform;

                        GameObject roofObjectsBack = Instantiate(roofObjects[0], new Vector3(lastXTile - 0.1F, lastYTile - RoofDown, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        roofObjectsBack.transform.parent = Parent.transform;
                    }
                    else
                    {
                        if (Section == -2)
                        {
                            (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == roofObjects[0]).GetSizeXYZ();
                            GameObject roofObjectsFront = Instantiate(roofObjects[0], new Vector3(lastXTile, lastYTile + 0.38F, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                            roofObjectsFront.transform.parent = Parent.transform;

                            GameObject roofObjectsBack = Instantiate(roofObjects[0], new Vector3(lastXTile, lastYTile + 0.38F, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                            roofObjectsBack.transform.parent = Parent.transform;
                        }
                        else
                        {
                            (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == roofObjects[0]).GetSizeXYZ();
                            GameObject roofObjectsFront = Instantiate(roofObjects[0], new Vector3(lastXTile, lastYTile - RoofDown, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                            roofObjectsFront.transform.parent = Parent.transform;

                            GameObject roofObjectsBack = Instantiate(roofObjects[0], new Vector3(lastXTile, lastYTile - RoofDown, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                            roofObjectsBack.transform.parent = Parent.transform;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Окна, боковые стены
                    if (i == 0)
                    {

                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == sideWallObjects[0]).GetSizeXYZ();
                        GameObject sideWallFront = Instantiate(sideWallObjects[0], new Vector3(OffRight + xWidth, lastYTile, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        sideWallFront.transform.parent = Parent.transform;

                        GameObject sideWallBack = Instantiate(sideWallObjects[0], new Vector3(OffRight + xWidth, lastYTile, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        sideWallBack.transform.parent = Parent.transform;

                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == windowInWallsObjects[0]).GetSizeXYZ();
                        GameObject windowInWallsBack = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile, OffСenterBack - zLength), Quaternion.Euler(new Vector3(0, 180, 0)));
                        windowInWallsBack.transform.parent = Parent.transform;

                        GameObject windowInWallsFront = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile, OffСenterFront - zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        windowInWallsFront.transform.parent = Parent.transform;

                        lastYTile += yHeight;
                    }
                    else if (i == Lenght - 1)
                    {
                   
                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == sideWallObjects[0]).GetSizeXYZ();
                        GameObject sideWallFront = Instantiate(sideWallObjects[0], new Vector3(lastXTile + OffLeft, lastYTile, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        sideWallFront.transform.parent = Parent.transform;

                        GameObject sideWallBack = Instantiate(sideWallObjects[0], new Vector3(lastXTile + OffLeft, lastYTile, zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        sideWallBack.transform.parent = Parent.transform;


                        (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == windowInWallsObjects[0]).GetSizeXYZ();
                        GameObject windowInWallsBack = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile, -2.4F), Quaternion.Euler(new Vector3(0, 180, 0)));
                        windowInWallsBack.transform.parent = Parent.transform;

                        GameObject windowInWallsFront = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile, OffСenterFront - zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                        windowInWallsFront.transform.parent = Parent.transform;

                        lastYTile += yHeight;
                    }
                    else
                    {
                        if (Section == -2)
                        {
                            if (j == Height)
                            {
                                (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == windowInWallsObjects[0]).GetSizeXYZ();
                                GameObject windowInWallsBack = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile - 1F, OffСenterBack - zLength), Quaternion.Euler(new Vector3(0, 180, 0)));
                                windowInWallsBack.transform.parent = Parent.transform;

                                (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == windowInWallLastObjects[0]).GetSizeXYZ();
                                GameObject windowInWallLastFront = Instantiate(windowInWallLastObjects[0], new Vector3(lastXTile, lastYTile - 0.5F, OffСenterFront - zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                                windowInWallLastFront.transform.parent = Parent.transform;

                            }
                            else
                            {
                                (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == windowInWallsObjects[0]).GetSizeXYZ();
                                GameObject windowInWallsBack = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile -1F, OffСenterBack - zLength), Quaternion.Euler(new Vector3(0, 180, 0)));
                                windowInWallsBack.transform.parent = Parent.transform;

                                GameObject windowInWallsFront = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile, OffСenterFront - zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                                windowInWallsFront.transform.parent = Parent.transform;
                                lastYTile += yHeight;
                            }
                        }
                        else
                        {
                            (xWidth, yHeight, zLength) = SizeObject.Find(so => so.obj == windowInWallsObjects[0]).GetSizeXYZ();
                            GameObject windowInWallsBack = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile, OffСenterBack - zLength), Quaternion.Euler(new Vector3(0, 180, 0)));
                            windowInWallsBack.transform.parent = Parent.transform;

                            GameObject windowInWallsFront = Instantiate(windowInWallsObjects[0], new Vector3(lastXTile, lastYTile, OffСenterFront - zLength), Quaternion.Euler(new Vector3(0, 0, 0)));
                            windowInWallsFront.transform.parent = Parent.transform;
                            lastYTile += yHeight;
                        }
                        
                    }
                    #endregion
                }
            }
            lastXTile += xWidth;
            countEntranceBlock++;
        }
    }

    void Start()
    {
        SetSizeObject();
        makeBuilding();
    }
}