using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class WaterRules : RuleTile<WaterRules.Neighbor> {
    public bool customField;

    public List<TileBase> sibings = new List<TileBase>();
    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Sibing = 3;
    }
    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Sibing: return sibings.Contains(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}