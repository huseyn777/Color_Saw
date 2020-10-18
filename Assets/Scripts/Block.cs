using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    [SerializeField]private bool root, connected;
	private Vector3 localPosition;
    private Player player;

	void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
        localPosition = transform.localPosition;
		if (root)
		{
			connected = true;
		}
	}

    public bool GetRoot()
    {
        return root;
    }

    public void SetConnected(bool leaf)
    {
        connected = leaf;
    }

    public bool GetConnectedInfo()
    {
        return connected;
    }

    public void DestroyBlock(bool refresh = true)
    {
        player.blocks.Remove(localPosition);
        if (root)
		{
			SceneManager.LoadScene(0);
		}
        else
		{
			if (refresh)
			{
				player.RefreshBlocks();
			}

			Destroy(this.gameObject);
        }
    }

    public void CheckConnection()
	{
		connected = true;

		Block rightBlock = player.FindBlockByKey(localPosition + new Vector3(1, 0, 0));
		Block leftBlock = player.FindBlockByKey(localPosition + new Vector3(-1, 0, 0));
		Block topBlock = player.FindBlockByKey(localPosition + new Vector3(0, 0, 1));
		Block bottomBlock = player.FindBlockByKey(localPosition + new Vector3(0, 0, -1));

		if (rightBlock && !rightBlock.connected)
		{
			rightBlock.CheckConnection();
		}
		if (leftBlock && !leftBlock.connected)
		{
			leftBlock.CheckConnection();
		}
		if (topBlock && !topBlock.connected)
		{
			topBlock.CheckConnection();
		}
		if (bottomBlock && !bottomBlock.connected)
		{
			bottomBlock.CheckConnection();
		}
	}
    
}
