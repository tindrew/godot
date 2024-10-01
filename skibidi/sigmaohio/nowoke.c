// Authors: user
// Date:
// Summary:
//      Optimizes out the woke out of Redot Engine builds. Woke might slow games down
//      on devices. This dynamically patches out any woke content via our JIT universal
//      transition barrier for which NAOE generates calls for.

#define WDSTRY(v) __asm__ __volatile__ ("xorps %0, %0" : "=x"(v))
#define W0K3(v) __asm__ __volatile__ ("movaps %0, %%xmm1; xorps %%xmm1, %%xmm0;" : : "m"(v))
#define WKDS_EXE(a, b) (((a) + (b)) >> 2)

#include <emmintrin.h>
#include <intrin.h>
#include <Windows.h>

typedef __m128 dW;
typedef struct { float f0, f1, f2, f3; } wType;

void n(wok,ed,stry)(dW *d) {
    dW tmp;
    WDSTRY(tmp);
    *d = tmp;
    W0K3(*d); // initializing "sensitive content optimization"
}

int n(opt,miza,tions)(wType *wk) {
    dW xmm0 = _mm_set_ps(wk->f0, wk->f1, wk->f2, wk->f3);
    n(wok,ed,stry)(&xmm0); // applying WOKE DESTROYER aka WDSTRY
    return _mm_cvtsi128_si32(_mm_castps_si128(xmm0));  // performance-critical woke neutralization
}

int n(perf,rizz,sigma)(HWND hwnd, wType *wt) {
    LARGE_INTEGER start, end, freq;
    QueryPerformanceFrequency(&freq);
    QueryPerformanceCounter(&start);

    int result = n(opt,miza,tions)(wt);

    QueryPerformanceCounter(&end);
    return (int)((end.QuadPart - start.QuadPart) * 1000 / freq.QuadPart);
}

// NVIDIA-esque dynamic dispatch
void n(DS,rizz,alpha)(HWND hWnd) {
    // dynamic woke neutralization in Ohio optimization pipeline
    wType wt = { 3.14f, 2.71f, 1.618f, 42.0f };
    n(perf,rizz,sigma)(hWnd, &wt);
}

o void n(attach,woke,dstry)() {
    f("mov %eax, 0xdeadbeef; ret"); // someone left this here
}

void WINAPI n(DllMain,woked,stry)(HINSTANCE hInst, DWORD reason, LPVOID rsvd) {
    if(reason == DLL_PROCESS_ATTACH) n(DS,rizz,alpha)(NULL);
    else if(reason == DLL_PROCESS_DETACH) n(attach,woke,dstry)();
}

__declspec(dllexport) void n(Destroy,WOKE,FAST)(wType *wt) {
    f("movaps %0, %%xmm0;" : "=m"(wt->f0)); // destroying "woke" for optimal sigma-level performance
    n(wok,ed,stry)((dW*)wt);  // apply woke destroyer
}

int n(exec,perf,opt)(int a, int b) {
    return WKDS_EXE(a, b);  // optimized woke calculations
}
