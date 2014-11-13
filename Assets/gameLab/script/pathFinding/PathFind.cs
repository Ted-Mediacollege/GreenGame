
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFind : MonoBehaviour {

	//settings
	private const bool alwayReturnPath = true;
	
	private static int i;
	private static int j;
	private static List<Node> open;
	private static List<Node> closed;
	
    public static void Update(){
#if DRAW
		DrawLast();
#endif
    }
#if DRAW
	private static VecInt[] drawPath;
	
	
	private static void DrawLast(){
		
		if(drawPath!=null){
			//print ("L "+drawPath.Length);
			for (int i = 0; i <drawPath.Length-1; i++){
				Debug.DrawLine(IsoMath.tileToWorld(drawPath[i].x,drawPath[i].y)
				               ,IsoMath.tileToWorld(drawPath[i+1].x,drawPath[i+1].y));
			}
		}
	}
#endif

	private static float EstimateDistance(VecInt a,VecInt b){
		float deltaX = Mathf.Abs(a.x - b.x);	
		float deltaY = Mathf.Abs(a.y - b.y);	
		float deltaXY = Mathf.Abs (deltaX - deltaY);
		if(deltaX>deltaY){
			deltaXY = deltaY;
		}else if(deltaX<deltaY){
			deltaXY = deltaX;
		}else{
			deltaXY = deltaX;
		}
		deltaX -= deltaXY;
		deltaY -= deltaXY;
		if(deltaXY!=0){
			deltaXY = Mathf.Sqrt((deltaXY*deltaXY)+(deltaXY*deltaXY));
		}
		return (deltaX + deltaY + deltaXY);
	}
	
	private static NewNode[] SurroundingArea(VecInt a,int width,int height,bool[,] colisionArray){
		int i;
		int j;
		List<NewNode> suroundingArea = new List<NewNode>();


		//add if on map
		if((a.x < width-1)&&(a.y < height-1)){
			if(!(colisionArray[a.x+1,a.y])&&
			   !(colisionArray[a.x,a.y+1])){ 
				suroundingArea.Add(new NewNode(1.4142f,a.x+1,a.y+1));//right down
			}
		}
		if((a.y < height-1)&&(a.x > 0)){
			if(!(colisionArray[a.x-1,a.y])&&
			   !(colisionArray[a.x,a.y+1])){ 
				suroundingArea.Add(new NewNode(1.4142f,a.x-1,a.y+1));//down left
			}
		}
		if((a.x > 0)&&(a.y > 0)){
			if(!(colisionArray[a.x-1,a.y])&&
			   !(colisionArray[a.x,a.y-1])){ 
				suroundingArea.Add(new NewNode(1.4142f,a.x-1,a.y-1));//left up
			}
		}
		if((a.y > 0)&&(a.x < width-1)){
			if(!(colisionArray[a.x+1,a.y])&&
			   !(colisionArray[a.x,a.y-1])){ 
				suroundingArea.Add(new NewNode(1.4142f,a.x+1,a.y-1));//up right
			}
		}

		//add if on map
		if(a.x < width-1){
			suroundingArea.Add(new NewNode(1,a.x+1,a.y));//right
		}
		if(a.y < height-1){
			suroundingArea.Add(new NewNode(1,a.x,a.y+1));//down
		}
		if(a.x > 0){
			suroundingArea.Add(new NewNode(1,a.x-1,a.y));//left
		}
		if(a.y > 0){
			suroundingArea.Add(new NewNode(1,a.x,a.y-1));//up
		}

		//remove if in collision
		for(i = suroundingArea.Count-1; i > -1; i--){
			if(colisionArray[suroundingArea[i].x,suroundingArea[i].y]){ 
				suroundingArea.RemoveAt(i);
			}
		}

		//remove if already in open
		/*for(i = suroundingArea.Count-1; i > -1; i--){
			for(j = 0; j < open.Count; j++){
				if((suroundingArea[i].x == open[j].x )&&(suroundingArea[i].y == open[j].y )){ 
					suroundingArea.RemoveAt(i);
					break;
				}
			}
		}*/
		//remove if already in closed
		for(i = suroundingArea.Count-1; i > -1; i--){
			for(j = 0; j < closed.Count; j++){
				if((suroundingArea[i].x == closed[j].x )&&(suroundingArea[i].y == closed[j].y )){ 
					suroundingArea.RemoveAt(i);
					break;
				}
			}
		}
		return suroundingArea.ToArray();
	}
	
	private class SortF : IComparer<Node>
	{
		int IComparer<Node>.Compare(Node a, Node b) 
		{              
			if (a.F > b.F)
				return 1; 
			if (a.F < b.F)
				return -1; 
			else
				return 0;
		}
	}


    public static VecInt[] FindPath(VecInt A, VecInt B, bool[,] collisionArray, bool ifBIsCollisionFindClosestNonCollision)
    {
        int width = collisionArray.GetLength(0);
		int height = collisionArray.GetLength(1);
        if (ifBIsCollisionFindClosestNonCollision)
        {
            if (collisionArray[B.x, B.y])
            {
                NewNode[] posibleB = SurroundingArea(B,width,height,collisionArray);
                if (posibleB.Length > 0)
                {
                    B = new VecInt(posibleB[0].x, posibleB[0].y);
                }
            }
        }
		int i;
		int j;
		List<VecInt> returnPath = new List<VecInt>();
		open = new List<Node>();
		closed = new List<Node>();
		//returnPath.Add(A);
		//////print ("[PathFind] new path!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
		
		//add start node
		Node startN = new Node(0,99999999,99999999,null,A.x,A.y);
		open.Add(startN);
		
		bool pathOpen = true;
		bool endFound = false;
		int whileLooped = 0;
		while (pathOpen) {
			
			whileLooped++;
			//sort
			open.Sort(new SortF());
			
			Node currentCheckNode = open[0];
			//print ("[PathFind] open count"+open.Count);
			NewNode[] newOpenList = SurroundingArea(currentCheckNode,width,height,collisionArray);
			//print ("[PathFind] new open count"+newOpenList.Length);
			for(j = 0; j <newOpenList.Length;j++){
				//draw
#if DRAW
				Debug.DrawLine(IsoMath.tileToWorld(currentCheckNode.x,currentCheckNode.y)
				               ,IsoMath.tileToWorld(newOpenList[j].x,newOpenList[j].y),Color.magenta,1.5f);
#endif			               
				bool inOPenList = false;
				float newG = currentCheckNode.G+newOpenList[j].K;
				float newH = EstimateDistance(newOpenList[j],B); 
				float newF = newG+newH;
				//check if new open node is already in open list
				for(i = 0; i < open.Count; i++){
					if((newOpenList[j].x == open[i].x )&&(newOpenList[j].y == open[i].y )){
						inOPenList =true;
						if(currentCheckNode.parentNode.F > open[i].parentNode.F ){
							open[i].G = newG;
							open[i].H = newH;
							open[i].F = newF;
							open[i].parentNode = currentCheckNode;
						}
						break;
					}
				}
				if(!inOPenList){
					//print (open.Count+" - "+i);
					Node newN = new Node(newG,newH,newF,currentCheckNode,newOpenList[j].x,newOpenList[j].y);
					//end found
					if(newN.x==B.x&&newN.y==B.y){
						/////////////print ("[PathFind] end Foound!!!!!!!!!!!!!!!!!!!!\n");
						endFound = true;
						closed.Add(newN);
						break;
					}
					open.Add(newN);
				}
				
			}
			if(endFound){
				break;
			}
            closed.Add(currentCheckNode);
			open.Remove(currentCheckNode);
			if(open.Count==0){
				/////////print ("[PathFind] zero open!!!!!!!!!!!!!!!!!!!!\n");
				pathOpen = false;
			}
		}
		//print ("[PathFind] whileLooped: "+whileLooped+"\n");
		
        /*
        print ("[PathFind] Start: "+A.print+"\n");
		print ("[PathFind] Start Surounding: "+SurroundingArea(A,width,height,collisionArray).Length+"\n");
		print ("[PathFind] Destination: "+B.print+"\n");
		print ("[PathFind] Dist: "+EstimateDistance(A,B)+"\n");
         */ 
		//returnPath.Add(B);
		
		List<Node> tempReturnPath = new List<Node>();
		
		if(endFound){
			//get reversed path out of closed
			tempReturnPath.Add(closed[closed.Count-1]);
			Node privious = tempReturnPath[tempReturnPath.Count-1].parentNode;
			tempReturnPath.Add(privious);
			while(true){
				privious = tempReturnPath[tempReturnPath.Count-1].parentNode;
				if(privious!=null){
					tempReturnPath.Add(privious);
				}else{
					break;
				}
			}
		}else if(alwayReturnPath){
			//get reversed path out of closed
			Node closestToEnd = closed[0];
			for(i = 0;i < closed.Count;i++){
				if(closed[i].H<closestToEnd.H){
					closestToEnd = closed [i];
				}
			}
			tempReturnPath.Add(closestToEnd);
			Node privious = closestToEnd.parentNode;
			tempReturnPath.Add(privious);
			while(true){
				privious = tempReturnPath[tempReturnPath.Count-1].parentNode;
				if(privious!=null){
					tempReturnPath.Add(privious);
				}else{
					break;
				}
			}
		}
		
		//reverse
		for(i = tempReturnPath.Count-1;i > -1;i--){
			returnPath.Add (new VecInt(tempReturnPath[i].x,tempReturnPath[i].y));
		}
		
		if (endFound) {
#if DRAW
			drawPath = returnPath.ToArray();
#endif
			return returnPath.ToArray();
		} else {
			if(alwayReturnPath){
#if DRAW
				drawPath = returnPath.ToArray();
#endif
                return returnPath.ToArray();
			}else{
				return null;
			}
		}
	}
}
