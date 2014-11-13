public class NewNode :VecInt
{
	public float K; // estimated Cost til end
	public NewNode(float k, int _x,int _y) :base(_x,_y)
	{
		K = k;
	}
}

