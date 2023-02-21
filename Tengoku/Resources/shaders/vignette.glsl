#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform vec4 colDiffuse;

out vec4 finalColor;

void main()
{
	vec2 uv = fragTexCoord.xy;
    uv *=  1.0 - uv.yx;
    
    float vig = uv.x*uv.y * 15.0;
    
    vig = pow(vig, 0.75);

    vec3 tc = texture(texture0, fragTexCoord).rgb;
    
    finalColor = vec4(vig * tc.x, vig * tc.y, vig * tc.z, 1.0); 
}