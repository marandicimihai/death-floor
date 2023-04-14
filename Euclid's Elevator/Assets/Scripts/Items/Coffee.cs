using UnityEngine;

public class Coffee : Item
{
    [SerializeField] float stunTime;
    [SerializeField] float stunDistance;
    [SerializeField] GameObject coffeeSpill;

    public override void UseItem(FpsController controller)
    {
        if (!Physics.Raycast(controller.settings.cam.transform.position, GameManager.Instance.enemy.position - controller.settings.cam.transform.position,
            Vector3.Distance(GameManager.Instance.enemy.position, transform.position), controller.settings.visionMask) &&
            controller.settings.cam.TryGetComponent(out Camera camera) && GameManager.Instance.enemy.TryGetComponent(out Collider col) &&
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds) && 
            Vector3.Distance(controller.transform.position, GameManager.Instance.enemy.position) <= stunDistance)
        {
            GameManager.Instance.enemyController.Stop(stunTime);
            Instantiate(coffeeSpill, GameManager.Instance.enemy.position, Quaternion.identity);
        }
    }
}
