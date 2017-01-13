package md5cab02428ab71a14fa1f8fa4bcb5ac2a0;


public class DragGridItemHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("CameraSuppl.DragGridItemHolder, CameraSuppl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DragGridItemHolder.class, __md_methods);
	}


	public DragGridItemHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DragGridItemHolder.class)
			mono.android.TypeManager.Activate ("CameraSuppl.DragGridItemHolder, CameraSuppl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
