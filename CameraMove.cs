using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class CameraMove : MonoBehaviour {
	public int rotateSpeed;
	public GameObject cube;
	public float cubex;
	public float cubez;
	public float radius = 200.0f;
	public Transform main;
	public float moveSpeed = 5;
	private float acceleration;
	public Color[] cubeColor = new Color[] {Color.red, Color.blue, Color.yellow, Color.green};
	public string[] cubeTag = new string[] {"Red", "Blue", "Yellow", "Green"};
	public GameObject miniCube;
	public GameObject explosion; 
	public ParticleSystem flare;
	public ParticleSystem sparks;
	public GameObject panel;
	public int score;
	public Text scoreText;
	public int time;
	public Text timeText;
	public AudioClip[] farts;
	public AudioClip bigFart;
	public AudioClip toilet;
	AudioSource audio;
	//private float multiplier;

	// Use this for initialization
	void Start () {
		StartCoroutine(Cube());
		StartCoroutine(Timer());
		panel.SetActive(false);
		score = 0;
		time = 60;
		audio= GetComponent<AudioSource>();
	}
	IEnumerator Timer(){
		Debug.Log ("sup");
		yield return new WaitForSeconds(1.0f);
		time -=1;
		timeText.text = time.ToString();
		if(time ==0){
			panel.SetActive(true);
		}
		if(time !=0){
			StartCoroutine(Timer());
		}
	}
	IEnumerator Cube(){

		GameObject newCube = (GameObject)Instantiate(cube, RandomInUnitCircle(radius), Quaternion.identity); 
		int colorNum = Random.Range(0,4);
		Renderer rend = newCube.GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Standard");
		rend.material.SetColor("_EmissionColor", cubeColor[colorNum]);
		newCube.tag = cubeTag[colorNum];
		yield return new WaitForSeconds(1.0f);
		if(time!= 0){
			StartCoroutine(Cube());
		}
	}
	public static Vector3 RandomInUnitCircle( float radius) 
	{
		Vector2 randomPointInCircle = Random.insideUnitCircle;
		randomPointInCircle *= radius;
		Vector3 point;
		point.x = randomPointInCircle.x;
		point.y = 0;
		point.z = randomPointInCircle.y;
		return point;
	}
	
	// Update is called once per frame
	void Update () {
		if(-20 <= transform.position.x  && transform.position.x <= 20 && -20 <= transform.position.z  && transform.position.z <= 20 ){
			transform.Translate(Vector3.forward * Time.deltaTime*moveSpeed);
		}
		else{
			if (transform.position.x >= 20){
				Vector3 temp = new Vector3(19.5f, transform.position.y, transform.position.z);
				transform.position = temp;
			}
			if (transform.position.x <= -20){
				Vector3 temp = new Vector3(-19.5f,transform.position.y, transform.position.z);
				transform.position = temp;
			}
			if (transform.position.z >= 20){
				Vector3 temp = new Vector3(transform.position.x, transform.position.y, 19.5f);
				transform.position = temp;
			}
			if (transform.position.z <= -20){
				Vector3 temp = new Vector3(transform.position.x, transform.position.y, -19.5f);
				transform.position = temp;
			}
		}
		//Ray ray = new Ray(this.transform.position, transform.forward);
		//RaycastHit hit;
		//if (Physics.Raycast (ray, out hit, 100)) {
		//	Debug.DrawLine (ray.origin, hit.point, Color.red);
		//	if(hit.collider.name.Contains("Cube")){
		//		for (var i = 0; i < Input.touchCount; i++){
		//			if(Input.GetTouch(i).phase == TouchPhase.Began){
		//				Destroy(hit.transform.gameObject);
		//				Debug.Log ("destroy");
		//			}
		//		}
		//	}
		//}	
		
		if (Input.acceleration.x > .1)
		{
			transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime * Input.acceleration.x), Space.World);
		}
		else if (Input.acceleration.x < -.1)
		{
			transform.Rotate(Vector3.up * (-rotateSpeed * Time.deltaTime * -Input.acceleration.x), Space.World);
		}
		//if (Input.acceleration.z > .2)
		//{
		//	acceleration = Input.acceleration.z-0.4F;
		//	transform.Rotate(Vector3.right * (rotateSpeed * Time.deltaTime * acceleration), Space.World);
		//}
		//else if (Input.acceleration.z < -0.2)
		//{
		//	acceleration = Input.acceleration.z;
		//	transform.Rotate(Vector3.right * (-rotateSpeed * Time.deltaTime * -acceleration), Space.World);
		//}
		//if(Input.GetKey("left")){
		//	transform.Rotate(-transform.up * Time.deltaTime * rotateSpeed, Space.World);
		//}
		//if(Input.GetKey("right")){
		//	transform.Rotate(transform.up * Time.deltaTime * rotateSpeed, Space.World);
		//}
		//if(Input.GetKey("up")){
		//	transform.Rotate(-transform.right * Time.deltaTime * rotateSpeed, Space.World);
		//}
		//if(Input.GetKey("down")){
		//	transform.Rotate(transform.right * Time.deltaTime * rotateSpeed, Space.World);
		//}
	
	}
	public void Blue(){
		StartCoroutine(Flare(Color.blue));
		Ray ray = new Ray(this.transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point, Color.red);
			if(hit.collider.tag.Contains("Blue")){
				for(int i = 0; i<5; i++){
					GameObject newMini = (GameObject)Instantiate(miniCube, hit.transform.position, Quaternion.identity);
					Destroy(newMini, 1.0f);

				}
				score += 1;
				scoreText.text = score.ToString();
				Destroy(hit.transform.gameObject);
				Debug.Log ("destroy");
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
				}
					
		}	
	}
	public void Red(){
		StartCoroutine(Flare(Color.red));
		Ray ray = new Ray(this.transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point, Color.red);
			if(hit.collider.tag.Contains("Red")){
				for(int i = 0; i<5; i++){
					GameObject newMini = (GameObject)Instantiate(miniCube, hit.transform.position, Quaternion.identity); 
					Renderer rend = newMini.GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Standard");
					rend.material.SetColor("_EmissionColor", Color.red);
					Destroy(newMini, 1.0f);

				}
				score += 1;
				scoreText.text = score.ToString();
				Destroy(hit.transform.gameObject);
				Debug.Log ("destroy");
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
			}
			
		}	
	}
	public void Yellow(){
		StartCoroutine(Flare(Color.yellow));
		Ray ray = new Ray(this.transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point, Color.red);
			if(hit.collider.tag.Contains("Yellow")){
				for(int i = 0; i<5; i++){
					GameObject newMini = (GameObject)Instantiate(miniCube, hit.transform.position, Quaternion.identity); 
					Renderer rend = newMini.GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Standard");
					rend.material.SetColor("_EmissionColor", Color.yellow);
					Destroy(newMini, 1.0f);

				}
				score += 1;
				scoreText.text = score.ToString();
				Destroy(hit.transform.gameObject);
				Debug.Log ("destroy");
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
			}
			
		}	
	}
	public void Green(){
		StartCoroutine(Flare(Color.green));
		Ray ray = new Ray(this.transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point, Color.red);
			if(hit.collider.tag.Contains("Green")){
				for(int i = 0; i<5; i++){
					GameObject newMini = (GameObject)Instantiate(miniCube, hit.transform.position, Quaternion.identity); 
					Renderer rend = newMini.GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Standard");
					rend.material.SetColor("_EmissionColor", Color.green);
					Destroy(newMini, 1.0f);

				}
				score += 1;
				scoreText.text = score.ToString();
				Destroy(hit.transform.gameObject);
				Debug.Log ("destroy");
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
			}
			
		}	
	}
	IEnumerator Flare(Color c){
		flare.startColor = c;
		sparks.startColor = c;
		flare.Play();
		yield return new WaitForSeconds(0.2f);
		flare.Stop();
	}
	public void LoadGame(){
		Debug.Log(Advertisement.isInitialized);
		if (Advertisement.IsReady())
		{
			Debug.Log("hi");
			Advertisement.Show();
		}
		//Application.LoadLevel("GameScene");
	}
	public void LoadStart(){
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
		Application.LoadLevel("StartScreen");
	}
	public void playFart(){
		int num = Random.Range(0,3);
		audio.PlayOneShot(farts[num]);
	}
	public void playBigFart(){
		audio.PlayOneShot(bigFart);
		audio.PlayOneShot(toilet);
	}
}
