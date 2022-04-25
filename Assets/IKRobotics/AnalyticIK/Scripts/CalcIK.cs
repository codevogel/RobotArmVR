using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AnalyticIK
{
public class CalcIK : MonoBehaviour {

    public Slider S_Slider;
    public Slider L_Slider;
    public Slider U_Slider;
    public Slider R_Slider;
    public Slider B_Slider;
    public Slider T_Slider;
    public static double[] theta = new double[6];

	[SerializeField] GameObject pxTextObject;
	[SerializeField] GameObject pyTextObject;
	[SerializeField] GameObject pzTextObject;
	[SerializeField] GameObject rxTextObject;
	[SerializeField] GameObject ryTextObject;
	[SerializeField] GameObject rzTextObject;
	Text pxText;
	Text pyText;
	Text pzText;
	Text rxText;
	Text ryText;
	Text rzText;
    
    private float L1, L2, L3, L4, L5, L6;
    private float C3;
	private float prevPx, prevPy, prevPz;  // to avoid impossible pose
	private float prevRx, prevRy, prevRz;

    // Use this for initialization
    void Start () {
        theta[0] = theta[1] = theta[2] = theta[3] = theta[4] = theta[5] = 0.0;
        L1 = 4.0f;
        L2 = 8.0f;
        L3 = 3.0f;
        L4 = 4.0f;
        L5 = 2.0f;
        L6 = 1.0f;
        C3 = 0.0f;

		pxText = pxTextObject.GetComponent<Text>();
		pyText = pyTextObject.GetComponent<Text>();
		pzText = pzTextObject.GetComponent<Text>();
		rxText = rxTextObject.GetComponent<Text>();
		ryText = ryTextObject.GetComponent<Text>();
		rzText = rzTextObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        float px, py, pz;
        float rx, ry, rz;
        float ax, ay, az, bx, by, bz;
        float asx, asy, asz, bsx, bsy, bsz;
        float p5x, p5y, p5z;
        float C1, C23, S1, S23;

        px = S_Slider.value;
		pxText.text = px.ToString ("F2");
        py = L_Slider.value;
		pyText.text = py.ToString ("F2");
        pz = U_Slider.value;
		pzText.text = pz.ToString ("F2");
        rx = R_Slider.value;
		rxText.text = rx.ToString ("F2");
        ry = B_Slider.value;
		ryText.text = ry.ToString ("F2");
        rz = T_Slider.value;
		rzText.text = rz.ToString ("F2");

        ax = Mathf.Cos(rz * 3.14f / 180.0f) * Mathf.Cos(ry * 3.14f / 180.0f);
        ay = Mathf.Sin(rz * 3.14f / 180.0f) * Mathf.Cos(ry * 3.14f / 180.0f);
        az = -Mathf.Sin(ry * 3.14f / 180.0f);
        
        p5x = px - (L5 + L6) * ax;
        p5y = py - (L5 + L6) * ay;
        p5z = pz - (L5 + L6) * az;

		if(p5x<0)p5x = 0;  //work area limitation
		theta[0] = Mathf.Atan2(p5y, p5x);

        C3 = (Mathf.Pow(p5x, 2) + Mathf.Pow(p5y, 2) + Mathf.Pow(p5z - L1, 2) - Mathf.Pow(L2, 2) - Mathf.Pow(L3+L4, 2))
            / (2 * L2 * (L3+ L4));
		if (C3 < -1 || C3 > 1) {
			reset_slider ();
			return;
		}
        theta[2] = Mathf.Atan2(Mathf.Pow(1 - Mathf.Pow(C3, 2), 0.5f), C3);

        float M = L2 + (L3+ L4) * C3;
        float N = (L3+ L4) * Mathf.Sin((float)theta[2]);
        float A = Mathf.Pow(p5x*p5x + p5y*p5y, 0.5f);
        float B = p5z - L1;
        theta[1] = Mathf.Atan2(M*A - N*B, N*A + M*B);

        C1 = Mathf.Cos((float)theta[0]);
		if (C1 < -1 || C1 > 1) {
			reset_slider ();
			return;
		}
        C23 = Mathf.Cos((float)theta[1] + (float)theta[2]);
		if (C23 < -1 || C23 > 1) {
			reset_slider ();
			return;
		}
        S1 = Mathf.Sin((float)theta[0]);
		if (S1 < -1 || S1 > 1) {
			reset_slider ();
			return;
		}
        S23 = Mathf.Sin((float)theta[1] + (float)theta[2]);
		if (S23 < -1 || S23 > 1) {
			reset_slider ();
			return;
		}

        bx = Mathf.Cos(rx * 3.14f / 180.0f) * Mathf.Sin(ry * 3.14f / 180.0f) * Mathf.Cos(rz * 3.14f / 180.0f)
            - Mathf.Sin(rx * 3.14f / 180.0f) * Mathf.Sin(rz * 3.14f / 180.0f);
        by = Mathf.Cos(rx * 3.14f / 180.0f) * Mathf.Sin(ry * 3.14f / 180.0f) * Mathf.Sin(rz * 3.14f / 180.0f)
            - Mathf.Sin(rx * 3.14f / 180.0f) * Mathf.Cos(rz * 3.14f / 180.0f);
        bz = Mathf.Cos(rx * 3.14f / 180.0f) * Mathf.Cos(ry * 3.14f / 180.0f);

        asx = C23 * (C1 * ax + S1 * ay) - S23 * az;
        asy = -S1 * ax + C1 * ay;
        asz = S23 * (C1 * ax + S1 * ay) + C23 * az;
        bsx = C23 * (C1 * bx + S1 * by) - S23 * bz;
        bsy = -S1 * bx + C1 * by;
        bsz = S23 * (C1 * bx + S1 * by) + C23 * bz;

        theta[3] = Mathf.Atan2(asy, asx);
        theta[4] = Mathf.Atan2(Mathf.Cos((float)theta[3]) * asx + Mathf.Sin((float)theta[3]) * asy, asz);
        theta[5] = Mathf.Atan2(Mathf.Cos((float)theta[3]) * bsy - Mathf.Sin((float)theta[3]) * bsx,
            -bsz / Mathf.Sin((float)theta[4]));

		prevPx = px;
		prevPy = py;
		prevPz = pz;
		prevRx = rx;
		prevRy = ry;
		prevRz = rz;
    }

	void reset_slider(){
		S_Slider.value = prevPx;
		L_Slider.value = prevPy; 
		U_Slider.value = prevPz;
		R_Slider.value = prevRx;
		B_Slider.value = prevRy;
		T_Slider.value = prevRz;
	}
}
}
