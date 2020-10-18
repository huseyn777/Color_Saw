using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Color color;
	private float speed = 10f;
	private float sensitivity = 0.035f;
    private bool movementReset;
	private Rigidbody rb;
	private Vector3 playerPos;
	private Vector3 playerStartPos;
	private Vector3 mousePos;
	private Vector3 mouseStartPos;
	private Vector3 direction;
	private Vector3 targetPos;

	public Dictionary<Vector3, Block> blocks = new Dictionary<Vector3, Block>();
	private int rootBlockCount;
	private GameManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
		CreateDictionary();
    }

    void Update() 
    {
		Movment();
    }

	private void FixedUpdate()
	{
		if (playerPos != targetPos)
		{
			targetPos = new Vector3(Mathf.Clamp(targetPos.x, -12f, 12f), 0, Mathf.Clamp(targetPos.z, -25f, 25f));
			rb.MovePosition(Vector3.MoveTowards(playerPos, targetPos, speed * Mathf.Max(1.5f, (playerPos - targetPos).magnitude) * Time.fixedDeltaTime));
		}
	}

	private void Movment()
	{
		playerPos = transform.position;
		mousePos = Input.mousePosition;

		if (Input.GetMouseButtonDown(0))
		{
			ResetMovement();
		}

        else if (Input.GetMouseButton(0))
		{
			Vector3 mouseDir = mousePos - mouseStartPos;
			if (direction == Vector3.zero)
			{
                if (mouseDir.magnitude < 1)
				{
					return;
				}
				direction = mouseDir.normalized;

				if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
				{
					direction = new Vector3(1, 0, 0);
				}
				else
				{
					direction = new Vector3(0, 0, 1);
				}
			}

			if (direction.x != 0 && mousePos.x != mouseStartPos.x)
			{
				Vector3 distance = new Vector3(Mathf.RoundToInt(mouseDir.x * sensitivity), 0, mouseDir.y);
				targetPos = playerStartPos + direction * distance.x;
			}
			else if (direction.z != 0 && mousePos.y != mouseStartPos.y)
			{
				Vector3 distance = new Vector3(mouseDir.x, 0, Mathf.RoundToInt(mouseDir.y * sensitivity));
				targetPos = playerStartPos + direction * distance.z;
			}
			else
			{
				targetPos = playerStartPos;
			}
		}

        if ((targetPos - playerPos).magnitude < 0.5)
		{
            if (!movementReset)
			{
				ResetMovement();
			}
		}
		else
		{
			movementReset = false;
		}
	}

    private void ResetMovement()
	{
		movementReset = true;
		direction = Vector3.zero;
		mouseStartPos = Input.mousePosition;
		playerStartPos = new Vector3(Mathf.RoundToInt(playerPos.x), 0, Mathf.RoundToInt(playerPos.z));
	}

	private void CreateDictionary()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			Block childBlock = child.GetComponent<Block>();
			blocks.Add(child.localPosition, childBlock);

			if (!childBlock.GetRoot())
			{
				child.GetComponent<MeshRenderer>().material.color = color;
			}
			else
			{
				rootBlockCount++;
			}
		}

		RefreshBlocks();
	}
	
	public void RefreshBlocks()
	{
		foreach (var block in blocks.Values)
		{
			if (block.GetRoot())
			{
				block.SetConnected(true);
			}
			else
			{
				block.SetConnected(false);
			}
			
		}

		foreach (var block in blocks.Values)
		{
			if (block.GetRoot())
			{
				block.CheckConnection();
			}
		}

		List<Block> notConnectedBlocks = new List<Block>();
		foreach (var block in blocks.Values)
		{
			if (!block.GetConnectedInfo())
			{
				notConnectedBlocks.Add(block);
			}
		}

		foreach (var block in notConnectedBlocks)
		{
			block.DestroyBlock(false);
		}
		LevelIsCompleted();
	}

	public Block FindBlockByKey(Vector3 key)
	{
		if(blocks.ContainsKey(key))
		{
			return blocks[key];
		}
		else
		{
			return null;
		}
	}

	public void LevelIsCompleted()
	{
		if(blocks.Count == rootBlockCount)
		{
			gm.NextLevel();
		}
	}
}
