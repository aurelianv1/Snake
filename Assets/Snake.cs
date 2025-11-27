using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments;

    public Transform segmentPrefab;
    public int initialSize = 4;

    // Variabile pentru swipe control
    private Vector2 _startTouchPosition, _endTouchPosition;
    public float swipeThreshold = 50f; // Prag minim pentru un swipe

    private void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);

        // Adaugă segmente inițiale
        for (int i = 1; i < initialSize; i++)
        {
            Grow();
        }
    }

    private void Update()
    {
        // Mișcare în funcție de tastele apăsate (pentru desktop)
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }

        // Detectează swipe-urile (pentru mobil)
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _endTouchPosition = touch.position;
                Vector2 swipeDelta = _endTouchPosition - _startTouchPosition;

                if (swipeDelta.magnitude > swipeThreshold)
                {
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        // Swipe orizontal
                        if (swipeDelta.x > 0 && _direction != Vector2.left)
                        {
                            _direction = Vector2.right;
                        }
                        else if (swipeDelta.x < 0 && _direction != Vector2.right)
                        {
                            _direction = Vector2.left;
                        }
                    }
                    else
                    {
                        // Swipe vertical
                        if (swipeDelta.y > 0 && _direction != Vector2.down)
                        {
                            _direction = Vector2.up;
                        }
                        else if (swipeDelta.y < 0 && _direction != Vector2.up)
                        {
                            _direction = Vector2.down;
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Mișcarea segmentelor (de la coadă la cap)
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        // Mișcarea capului în direcția setată
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        // Creează un nou segment și îl adaugă în lista de segmente
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    private void ResetState()
    {
        // Distruge toate segmentele, în afară de cap
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        // Resetează lista de segmente și poziția
        _segments.Clear();
        _segments.Add(this.transform);
        this.transform.position = Vector3.zero;

        // Recreează segmentele inițiale
        for (int i = 1; i < initialSize; i++)
        {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Crește șarpele când mănâncă "Food"
        if (other.tag == "Food")
        {
            Grow();
        }
        // Resetează starea când șarpele lovește un obstacol
        else if (other.tag == "Obstacle")
        {
            ResetState();
        }
    }
}
