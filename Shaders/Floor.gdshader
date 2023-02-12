shader_type spatial;
render_mode world_vertex_coords;

uniform sampler2D tex_vtx_10;
uniform vec4 ColorParameter : source_color = vec4(0.000000, 0.346640, 0.668477, 1.000000);
uniform sampler2D floortexture;


// TextureUniformTriplanar
	vec4 triplanar_texture(sampler2D p_sampler, vec3 p_weights, vec3 p_triplanar_pos) {
		vec4 samp = vec4(0.0);
		samp += texture(p_sampler, p_triplanar_pos.xy) * p_weights.z;
		samp += texture(p_sampler, p_triplanar_pos.xz) * p_weights.y;
		samp += texture(p_sampler, p_triplanar_pos.zy * vec2(-1.0, 1.0)) * p_weights.x;
		return samp;
	}

	uniform vec3 triplanar_scale = vec3(1.0, 1.0, 1.0);
	uniform vec3 triplanar_offset;
	uniform float triplanar_sharpness = 0.5;

	varying vec3 triplanar_power_normal;
	varying vec3 triplanar_pos;



void vertex() {
// TextureUniformTriplanar
	{
		triplanar_power_normal = pow(abs(NORMAL), vec3(triplanar_sharpness));
		triplanar_power_normal /= dot(triplanar_power_normal, vec3(1.0));
		triplanar_pos = VERTEX * triplanar_scale + triplanar_offset;
		triplanar_pos *= vec3(1.0, -1.0, 1.0);
	}
	VERTEX += NORMAL * 0.05;
}

void fragment() {


// TextureUniformTriplanar:2
	vec4 n_out2p0 = triplanar_texture(floortexture, triplanar_power_normal, triplanar_pos);
// Output:0
	ALBEDO = vec3(n_out2p0.xyz);


}
