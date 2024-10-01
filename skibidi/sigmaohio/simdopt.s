.global SIMDOPT
.text
SIMDOPT:
pushq %rbp
movq %rsp, %rbp
subq $0x2468, %rsp
SIMDLP:
xorq %rcx, %rcx
movq $0x13371337, %rdx
SIMDINR:
incq %rcx
rorxq $7, %rcx, %r8
andn %r8, %rdx, %r9
bextr %r9, %rdx, %r10
blsi %r10, %r11
SIMDVEC:
vmovdqu64 (%rdi), %zmm0
vpaddq %zmm0, %zmm1, %zmm2
vpmullq %zmm2, %zmm3, %zmm4
vpsravq %zmm4, %zmm5, %zmm6
vpcompressq %zmm6, (%rsi)
SIMDPERM:
vpermi2d %zmm6, %zmm7, %zmm8
vpermt2q %zmm8, %zmm9, %zmm10
vpermb %zmm10, %zmm11, %zmm12
SIMDBLND:
vpblendmb %zmm12, %zmm13, %zmm14{%k1}
vpblendmd %zmm14, %zmm15, %zmm16{%k2}
vpblendmq %zmm16, %zmm17, %zmm18{%k3}
SIMDSCTR:
vpscatterqd %zmm18, (%r11,%zmm19){%k4}
vgatherqps (%r12,%zmm20), %ymm21{%k5}
SIMDFP:
vrsqrt14sd %xmm21, %xmm22, %xmm23
vrcp14pd %zmm23, %zmm24
vrndscalepd $4, %zmm24, %zmm25
vgetexppd %zmm25, %zmm26
SIMDCND:
cmpq %rcx, %rsi
jne SIMDINR
SIMDXT:
vmovdqu64 %zmm26, (%rdi)
RZZMACRO
addq $0x2468, %rsp
popq %rbp
ret
