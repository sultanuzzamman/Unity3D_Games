#pragma strict
public var proj:GameObject;
public var firerate:float=0.5;
internal var nextfire: float;
public var speed:float;
function Start () {

}

function Update () {

if(Input.GetKey("f")&&Time.time>nextfire){
nextfire=Time.time+firerate;
var clone:GameObject=Instantiate(proj,transform.position,transform.rotation);
clone.GetComponent.<Rigidbody>().velocity=transform.TransformDirection(Vector3(0,0,speed));
}

}