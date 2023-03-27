using UnityEngine;

public class Coffee : Item
{
    [SerializeField] float stunTime;
    [SerializeField] float stunDistance;

    public override void UseItem(FpsController controller)
    {
        if (GameManager.instance.spawnEnemy && !Physics.Raycast(controller.settings.cam.transform.position, GameManager.instance.enemy.position - controller.settings.cam.transform.position,
            Vector3.Distance(GameManager.instance.enemy.position, transform.position), controller.settings.visionMask) &&
            controller.settings.cam.TryGetComponent(out Camera camera) && GameManager.instance.enemy.TryGetComponent(out Collider col) &&
            GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), col.bounds) && 
            Vector3.Distance(controller.transform.position, GameManager.instance.enemy.position) <= stunDistance)
        {
            GameManager.instance.enemyController.Stop(stunTime);
        }
    }
}
