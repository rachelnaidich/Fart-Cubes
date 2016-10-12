public class CameraMove : MonoBehaviour {
	public int cubeNum;
	public int actualCubeNum;

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
	public Text highScoreText;
	public int score;
	public Text scoreText;
	public int time;
	public Text timeText;
	public AudioClip[] farts;
	public AudioClip bigFart;
	public AudioClip toilet;
	AudioSource audio;
	public Slider bar;
	public Text plusTime;
	public Color plusColor;

	// Use this for initialization
	void Start () {
		for(int a = 0; a < cubeNum; a++){
			Cube();
		}
		actualCubeNum = cubeNum;
		StartCoroutine(Timer());
		panel.SetActive(false);
		plusColor = new Color(1,1,1,0);
		plusTime.color = plusColor;
		score = 0;
		time = 10;
		audio= GetComponent<AudioSource>();
		bar.maxValue = 0.99f;
		bar.minValue = 0.00f;
		bar.value = 0;
	}
	IEnumerator Timer(){
		yield return new WaitForSeconds(1.0f);
		time -=1;
		timeText.text = time.ToString();
		if(time ==0){
			panel.SetActive(true);
			highScoreText.text = "High Score: " + HighScore.StoreHighscore(score).ToString();
		}
		if(time !=0){
			StartCoroutine(Timer());
		}
	}
	void Cube(){
		GameObject newCube = (GameObject)Instantiate(cube, RandomInUnitCircle(radius), Quaternion.identity); 
		int colorNum = Random.Range(0,4);
		Renderer rend = newCube.GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Standard");
		rend.material.SetColor("_EmissionColor", cubeColor[colorNum]);
		newCube.tag = cubeTag[colorNum];
		actualCubeNum +=1;

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
		if(actualCubeNum< cubeNum){
			Cube();
		}
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
				actualCubeNum -=1;
				StartCoroutine(ChangeBarVal());
				Destroy(hit.transform.gameObject);
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
				highScoreText.text = "High Score: " + HighScore.StoreHighscore(score).ToString();
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
				actualCubeNum -=1;
				StartCoroutine(ChangeBarVal());
				Destroy(hit.transform.gameObject);
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
				highScoreText.text = "High Score: " + HighScore.StoreHighscore(score).ToString();
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
				actualCubeNum -=1;
				StartCoroutine(ChangeBarVal());
				Destroy(hit.transform.gameObject);
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
				highScoreText.text = "High Score: " + HighScore.StoreHighscore(score).ToString();
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
				actualCubeNum -=1;
				StartCoroutine(ChangeBarVal());
				Destroy(hit.transform.gameObject);
				playFart();
			}
			else{
				playBigFart();
				panel.SetActive(true);
				highScoreText.text = "High Score: " + HighScore.StoreHighscore(score).ToString();
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
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
		Application.LoadLevel("GameScene");
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
	IEnumerator ChangeBarVal(){
		if(bar.value < 0.9f){
			for(int x = 0; x < 20; x++){
				bar.value += 0.01f;
				yield return new WaitForSeconds(0.01f);
			}
		}
		else{
			StartCoroutine(AddTime(10));
			for(int x = 0; x < 99; x++){
				bar.value-= 0.03f;
				yield return new WaitForSeconds(0.01f);
			}
		}
	}
	IEnumerator AddTime(int t){
		time+=t;
		for(int x = 0; x < 3; x++){
			Debug.Log(x);
			plusColor.a = 1;
			plusTime.color = plusColor;
			yield return new WaitForSeconds(0.3f);
			plusColor.a = 0;
			plusTime.color = plusColor;
		}
	}
}
