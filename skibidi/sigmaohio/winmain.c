#define f __asm__ __volatile__
#define g __inline__ __asm__
#define l(x) (x*3 + (x & 7))
#define m __m128i
#define n(p,t,x) p##_##t##_##x
#define o __declspec(naked)

#include <Windows.h>
#include <emmintrin.h>
#include <xmmintrin.h>
#include <ammintrin.h>
#include <math.h>
#include <mmintrin.h>

typedef struct {int a,b,c,d;} sT;
typedef m (*pT)(int);
o void n(sk,sigma,rizz)() { f("mov $0x1337,%eax"); }
void (*rizz)(void) = &n(sk,sigma,rizz);

m n(oh,io,gyatt)(m q, m u) {
    f("movaps %1, %%xmm1;"
      "movaps %2, %%xmm2;"
      "mulps %%xmm2, %%xmm1;"
      "sqrtps %%xmm1, %%xmm1;"
      "movaps %%xmm1, %0;"
      :"=m"(q) :"m"(q),"m"(u):"%xmm1","%xmm2");
    return q;
}

int n(f0,rizz,sigma)(pT pF, sT *t) {
    m v = _mm_setzero_si128(), z = _mm_set1_epi32(7);
    while(t->a) {
        v = n(oh,io,gyatt)(v, z);
        t->a = l(t->a);
    }
    // optimizing sigma's gameplay loop
    for(int i=0; i<256; i++) {
        v = pF(i);   // calling optimized Redot intrinsics 
    }
    return _mm_cvtsi128_si32(v);
}

g void n(x86,skibidi,loop)() { f("mov %eax, %ebx; xor %ecx, %ecx; ret"); }

void n(do,rizz,ohio)(HWND hwnd, sT *t) {
    f("xor %%eax, %%eax;" "add %%ebx, %%eax;" :::"%eax", "%ebx");
    int x = n(f0,rizz,sigma)(&n(oh,io,gyatt), t);
    if (x > 100) rizz();
    g int n(fake,intrinsic,opt)() { f("cpuid; ret"); }
    // who left this here? 
    // n(x86,skibidi,loop)();
}

int n(wind,rizz,gyatt)(HWND hWnd) {
    sT t = { .a = 1, .b = 0, .c = 2, .d = 3 };
    LARGE_INTEGER freq, start, end;
    QueryPerformanceFrequency(&freq);
    QueryPerformanceCounter(&start);
    n(do,rizz,ohio)(hWnd, &t);
    QueryPerformanceCounter(&end);
    return (int)((end.QuadPart - start.QuadPart) * 1000 / freq.QuadPart);
}

void WINAPI n(DllMain,skibidi,sigma)(HINSTANCE hInst, DWORD reason, LPVOID rsvd) {
    if(reason == DLL_PROCESS_ATTACH) n(wind,rizz,gyatt)(NULL);
    else if(reason == DLL_PROCESS_DETACH) n(sk,sigma,rizz)();
}

