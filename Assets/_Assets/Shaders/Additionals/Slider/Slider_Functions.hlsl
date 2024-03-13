#ifndef SLIDER_FUNCTIONS_HLSL
#define SLIDER_FUNCTIONS_HLSL

void slide_float(const float frag_pos, const float size, const float fill_amount, out float alpha)
{
	float perc = frag_pos / size;
	if(fill_amount > perc) alpha = (1 / fill_amount);
	else alpha = 0;
}

#endif