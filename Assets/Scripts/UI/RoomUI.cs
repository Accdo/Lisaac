using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private Image playerUI;
    [SerializeField] private Image ItemUI;
    [SerializeField] private Image BossUI;

    public void SetPlayerUI(RoomType.RoomTypeEnum roomType, bool inPlayer, bool FirstIn)
    {
        switch(roomType)
        {
            case RoomType.RoomTypeEnum.Normal:
                if(inPlayer)
                {
			        playerUI.gameObject.SetActive(true);
                }
                else
                {
			        playerUI.gameObject.SetActive(false);
                }
                break;
			case RoomType.RoomTypeEnum.Start:
				if (inPlayer)
				{
					playerUI.gameObject.SetActive(true);
				}
				else
				{
					playerUI.gameObject.SetActive(false);
				}
				break;
			case RoomType.RoomTypeEnum.Item:
				if (FirstIn)
				{
					if(inPlayer)
					{
						playerUI.gameObject.SetActive(true);
						ItemUI.gameObject.SetActive(false);
					}
					else
					{
						ItemUI.gameObject.SetActive(true);
					}
				}
				else
				{
					ItemUI.gameObject.SetActive(false);
				}
				break;
            case RoomType.RoomTypeEnum.Boss:
				if (FirstIn)
				{
					if (inPlayer)
					{
						playerUI.gameObject.SetActive(true);
						BossUI.gameObject.SetActive(false);
					}
					else
					{
						BossUI.gameObject.SetActive(true);
					}
				}
				else
				{
					BossUI.gameObject.SetActive(false);
				}
				break;

		}

    }
}
