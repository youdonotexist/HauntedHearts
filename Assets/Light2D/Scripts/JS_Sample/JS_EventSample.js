// You must move the "Light2D.cs" script to a folder labeled "Plugins". The "Plugins" folder
// needs to be placed in your assets root folder to work.

// UNCOMMENT THIS FOR THE CODE AFTER YOU HAVE FOLLLOWED THE ABOVE INSTRUCTIONS
/*
function Start () 
{
	Light2D.RegisterEventListener(LightEventListenerType.OnEnter, OnEnterEvent);
	Light2D.RegisterEventListener(LightEventListenerType.OnStay, OnStayEvent);
	Light2D.RegisterEventListener(LightEventListenerType.OnExit, OnExitEvent);
	
}

function OnEnterEvent(l2d : Light2D, go : GameObject)
{
	if(go.GetInstanceID() == gameObject.GetInstanceID())
		l2d.lightColor = Color.blue;
}

function OnStayEvent(l2d : Light2D, go : GameObject)
{
	// Do stuffs
}

function OnExitEvent(l2d : Light2D, go : GameObject)
{
	if(go.GetInstanceID() == gameObject.GetInstanceID())
		l2d.lightColor = Color.red;
}
*/