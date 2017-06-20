using UnityEngine;
using ChrsUtils.EasingEquations;

[CreateAssetMenu (menuName = "Easing Properties")]
public class EasingProperties : ScriptableObject
{
    [SerializeField] private Easing.FunctionType _Easing;
    public Easing.Function ScaleDown { get { return Easing.GetFunctionWithTypeEnum(_Easing); }}
}
