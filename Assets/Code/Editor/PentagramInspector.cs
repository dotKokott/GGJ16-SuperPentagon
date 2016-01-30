using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor( typeof( Pentagram ) )]
public class PentagramInspector : Editor {

    private Pentagram mTarget;
    private ReorderableList list;
    private List<PentaInfo> values;

    [InitializeOnLoadMethod]
    private void Initialize() {
        if ( list == null ) {
            mTarget = target as Pentagram;
            values = mTarget.Qu;

            list = new ReorderableList( values, typeof( PentaInfo ), true, true, true, true );
            list.elementHeight *= 2;
            list.onAddCallback = new ReorderableList.AddCallbackDelegate( OnAdd );
            list.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate( OnRemove );
            list.drawElementCallback = new ReorderableList.ElementCallbackDelegate( OnDrawElement );

        }
    }

    private void OnDrawElement( Rect rect, int index, bool isActive, bool isFocused ) {
        EditorGUI.BeginChangeCheck();

        var item = values[index];

        rect.y += 3f;
        rect.height -= 7f;

        var hh = rect.height / 2;
        var hw = rect.width / 2;

        rect.height = hh;
        item.Mode = (EPentaMode)EditorGUI.EnumPopup( rect, item.Mode );

        rect.y += hh;

        if ( item.Mode == EPentaMode.Instant ) {
            var color = ColorExtension.HSVToRGB( item.Hue, 1, 1 );
            color = EditorGUI.ColorField( rect, "Hue", color );
            item.Hue = color.ToHSV().x;
        } else if ( item.Mode == EPentaMode.Timed ) {
            rect.width = hw;
            var color = ColorExtension.HSVToRGB( item.Hue, 1, 1 );
            color = EditorGUI.ColorField( rect, "Hue", color );
            item.Hue = color.ToHSV().x;

            rect.x += hw;
            item.Time = EditorGUI.FloatField( rect, "Time", item.Time );
        } else if ( item.Mode == EPentaMode.Wait ) {
            item.Time = EditorGUI.FloatField( rect, "Time", item.Time );
        }

        if ( EditorGUI.EndChangeCheck() ) {
            FillQueue();
        }
    }


    private void OnAdd( ReorderableList list ) {
        list.list.Add( new PentaInfo() );
        FillQueue();
    }

    private void OnRemove( ReorderableList list ) {
        list.list.RemoveAt( list.index );
        FillQueue();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Initialize();
        list.DoLayoutList();
        //FillQueue();
    }

    private void FillQueue() {
        //mTarget.Qu.Clear();
        //foreach ( var item in values ) {
        //    mTarget.Qu.Add( item );
        //}
    }
}
