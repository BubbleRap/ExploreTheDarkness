Shader "Custom/TransparentInvisibleShadowCaster" {
{
Subshader
{
UsePass "VertexLit/SHADOWCOLLECTOR"
UsePass "VertexLit/SHADOWCASTER"
}
 
Fallback off
}
