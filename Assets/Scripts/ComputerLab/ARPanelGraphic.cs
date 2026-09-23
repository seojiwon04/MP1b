using UnityEngine;
using UnityEngine.UI;
namespace EscapeRoom.ARInterface
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class ARPanelGraphic : MaskableGraphic
    {
        public Color tint=new Color(.38f,.76f,1f,.13f);
        public Color lineColor=new Color(1,1,1,.9f);
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();float w=rectTransform.rect.width,h=rectTransform.rect.height;
            Vector2 P(float x,float y)=>new Vector2((x-.5f)*w,(y-.5f)*h);
            var outline=new[]{P(.025f,0),P(.975f,0),P(1,.04f),P(1,.96f),P(.975f,1),P(.025f,1),P(0,.96f),P(0,.04f)};
            vh.AddVert(Vector3.zero,tint,Vector2.zero);
            for(int i=0;i<8;i++)vh.AddVert(outline[i],tint,Vector2.zero);
            for(int i=0;i<8;i++)vh.AddTriangle(0,1+i,1+(i+1)%8);
            void Line(Vector2 a,Vector2 b,float thickness,Color c){Vector2 n=new Vector2(-(b-a).y,(b-a).x).normalized*thickness*.5f;int k=vh.currentVertCount;vh.AddVert(a-n,c,Vector2.zero);vh.AddVert(a+n,c,Vector2.zero);vh.AddVert(b+n,c,Vector2.zero);vh.AddVert(b-n,c,Vector2.zero);vh.AddTriangle(k,k+1,k+2);vh.AddTriangle(k,k+2,k+3);}
            for(int i=0;i<8;i++)Line(outline[i],outline[(i+1)%8],1.3f,lineColor*.7f);
            for(int sx=0;sx<2;sx++)for(int sy=0;sy<2;sy++){
                Vector2 Q(float x,float y)=>P(sx==0?x:1-x,sy==0?y:1-y);
                Line(Q(.012f,.12f),Q(.012f,.05f),3,lineColor);
                Line(Q(.012f,.05f),Q(.035f,.018f),4,lineColor);
                Line(Q(.035f,.018f),Q(.11f,.018f),3,lineColor);
            }
            foreach(float x in new[]{.38f,.58f}){Line(P(x,.97f),P(x+.04f,.97f),2,lineColor);Line(P(x,.015f),P(x+.04f,.015f),2,lineColor);}
            foreach(float x in new[]{.01f,.99f})Line(P(x,.4f),P(x,.6f),1.5f,lineColor);
        }
    }
}
