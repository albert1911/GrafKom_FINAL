#version 430

out vec4 outputColor;

struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform DirLight dirLight;

struct PointLight {
    vec3 position;

    float constant;
    float linear;
    float quadratic;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
// #define NR_POINT_LIGHTS "jumlah point light"
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];


in vec3 normal;
in vec3 FragPos;

uniform vec3 objectColor;

// UNTUK LIGHTING BIASA

// uniform vec4 objColor;
// uniform vec3 lightPos;
// uniform vec3 lightColor;

//

uniform vec3 viewPos;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256);
    
    // combine results
    vec3 ambient  = light.ambient  * objectColor;
    vec3 diffuse  = light.diffuse  * diff * objectColor;
    vec3 specular = light.specular * spec * 
    objectColor;
    return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);

    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);

    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0),256);
    
    // attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));
    
    //combine results
    vec3 ambient  = light.ambient  * objectColor;
    vec3 diffuse  = light.diffuse  * diff * objectColor;
    vec3 specular = light.specular * spec * objectColor;
    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
}

void main() {
    // LIGHTING BIASA

    // float ambientStrength = 0.1;
    // vec3 ambient = ambientStrength * lightColor;

    // vec3 norm = normalize(normal);
    // vec3 lightDir = normalize(lightPos - FragPos);

    // float diff = max(dot(norm, lightDir), 0);
    // vec3 diffuse = diff * lightColor;

    // float specularStrength = 1;
    // vec3 viewDir = normalize(viewPos - FragPos);
    // vec3 reflectDir = reflect(-lightDir, norm);
    // float spec = pow(max(dot(viewDir, reflectDir), 0), 32);
    // vec3 specular = specularStrength * spec * lightColor;

    // vec3 result = (ambient + diffuse + specular) * vec3(objColor);

    // outputColor = vec4(result, objColor.w);

    //

    // LIGHTING YANG 3 JENIS

    vec3 normal = normalize(normal);
    vec3 viewPos = normalize(viewPos - vec3(FragPos));

    
	vec3 result = vec3(0, 0, 0);

    // DIRECTIONAL LIGHT
    vec3 directionalLight = CalcDirLight(dirLight, normal, viewPos);
    result += directionalLight;

    // POINT LIGHTS
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalcPointLight(pointLights[i], normal, vec3(FragPos), viewPos);

	outputColor = vec4(result, 1.0);

    //
}