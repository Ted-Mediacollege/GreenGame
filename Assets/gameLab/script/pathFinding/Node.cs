public class Node :VecInt
{
	public float G; // movement from start
	public float H; // estimated Cost til end
	public float F; // weight
	public Node parentNode;
	public Node(float g, float h,float f, Node parent, int _x,int _y) :base(_x,_y)
	{
		G = g;
		H = h;
		F = f;
		parentNode = parent;
	}
}