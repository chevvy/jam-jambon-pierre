shader_type canvas_item;

uniform sampler2D noise: source_color;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float speed_factor = TIME / 2.0;

	float thickness_factor = 0.2; // float thickness_factor = (1.0 - pow(COLOR.a, 2));

	vec2 fake_UV = vec2(abs(0.5 - UV.y) * thickness_factor, COLOR.a + speed_factor);

	vec4 n = texture(noise, fake_UV);

	float grey = 1.0 - (0.2126 * n.r + 0.7152 * n.g + 0.0722 * n.b);

	if (grey < 0.55) { COLOR.a = COLOR.a * pow(grey, 2.0); }

	if (grey < 0.5) { discard; }
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
