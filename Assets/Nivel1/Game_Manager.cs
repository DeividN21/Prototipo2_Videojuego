using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour{

	public GameObject canvas;

	[System.Serializable]
	public class Puzzle
	{

		public int winValue;
		public int curValue;

		public int width;
		public int height;
		public Piece[,] pieces;

	}


	public Puzzle puzzle;


	// Use this for initialization
	void Start () {
		
		canvas.SetActive(false);

		puzzle = new Puzzle();
		Vector2 dimensions = CheckDimensions ();

		puzzle.width = (int)dimensions.x;
		puzzle.height = (int)dimensions.y;


		puzzle.pieces = new Piece[puzzle.width, puzzle.height];


		foreach (var Piece in GameObject.FindGameObjectsWithTag("Piece")) {

			puzzle.pieces [(int)Piece.transform.position.x, (int)Piece.transform.position.y] = Piece.GetComponent<Piece> ();

		}


		foreach (var item in puzzle.pieces) {
			if (item != null) {
                Debug.Log(item.gameObject.name);
            } else {
                Debug.Log("Found a null item in puzzle.pieces.");
            }
		}


		puzzle.winValue = GetWinValue ();

		Shuffle ();

		puzzle.curValue=Sweep ();


	}



	public int Sweep()
	{
		int value = 0;

		for (int h = 0; h < puzzle.height; h++) {
			for (int w = 0; w < puzzle.width; w++) {


				//compares top
				if(h!=puzzle.height-1)
				if (puzzle.pieces [w, h].values [0] == 1 && puzzle.pieces [w, h + 1].values [2] == 1)
					value++;


				//compare right
				if(w!=puzzle.width-1)
				if (puzzle.pieces [w, h].values [1] == 1 && puzzle.pieces [w + 1, h].values [3] == 1)
					value++;


			}
		}

		return value;

	}

	public void Win()
	{

		canvas.SetActive (true);
	}

	public int QuickSweep(int w, int h)
	{
		int value = 0;

		//compares top
				if(h!=puzzle.height-1)
				if (puzzle.pieces [w, h].values [0] == 1 && puzzle.pieces [w, h + 1].values [2] == 1)
					value++;


				//compare right
				if(w!=puzzle.width-1)
				if (puzzle.pieces [w, h].values [1] == 1 && puzzle.pieces [w + 1, h].values [3] == 1)
					value++;

				//compare left
				if(w != 0)
				if (puzzle.pieces [w, h].values [3] == 1 && puzzle.pieces [w - 1, h].values [1] == 1)
					value++;
				
				//compare bottom
				if (h != 0)
				if (puzzle.pieces [w, h].values [2] == 1 && puzzle.pieces [w, h-1].values [0] == 1)
					value++;


				return value;
	}

	int GetWinValue()
	{
		int winValue = 0;
		foreach (var piece in puzzle.pieces) {


			foreach (var j in piece.values) {
				winValue += j;
			}


		}

		winValue /= 2;

		return winValue;

	}

	void Shuffle()
	{
		foreach (var piece in puzzle.pieces) {

			int k = Random.Range (0, 4);

			for (int i = 0; i < k; i++) {
				piece.RotatePiece ();
			}


		}
	}


	Vector2 CheckDimensions()
	{
		Vector2 aux = Vector2.zero;

		GameObject[] pieces = GameObject.FindGameObjectsWithTag ("Piece");

		foreach (var p in pieces) {
			if (p.transform.position.x > aux.x)
				aux.x = p.transform.position.x;

			if (p.transform.position.y > aux.y)
				aux.y= p.transform.position.y;
		}

		aux.x++;
		aux.y++;

		return aux;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void NextLevel(string Minimapa)
	{

		// Guardar la escena actual para verificar si es el minimapa
        string escenaActual = SceneManager.GetActiveScene().name;

        // Cargar la escena especificada
        SceneManager.LoadScene(Minimapa);

        // Verificar si la escena cargada es el minimapa
        if (escenaActual == "Minimapa")
        {
            // Realizar acciones adicionales específicas del minimapa

            // Abrir el camino hacia el siguiente nivel
            if (Variables.maxNivel < 3 && Variables.nivel == 1 && Variables.estadoPos2 == 0)
            {
                Variables.maxNivel++;
                Variables.estadoPos2 = 2;
            }

            // Restablecer la capacidad de mover al jugador
            FindObjectOfType<MinimapaController>().RestablecerPuedoMover();
        }

	}
}