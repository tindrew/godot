.global GYATTFNC
.text
GYATTFNC:
pushq %rbp
movq %rsp, %rbp
subq $0x1337, %rsp
GYATTLP:
xorq %rcx, %rcx
movq $0x42069, %rdx
GYATTINR:
incq %rcx
rorxq $13, %rcx, %r8
pdepq %r8, %rdx, %r9
pextq %r9, %rdx, %r10
blsrq %r10, %r11
tzcntq %r11, %r12
lzcntq %r12, %r13
GYATTVEC:
vmovdqa64 (%rdi), %zmm0
vpaddd %zmm0, %zmm1, %zmm2
vpmaddwd %zmm2, %zmm3, %zmm4
vpmaddubsw %zmm4, %zmm5, %zmm6
vpermw %zmm6, %zmm7, %zmm8
GYATTCRYP:
vaesenc %zmm8, %zmm9, %zmm10
vclmulqdq $0x11, %zmm10, %zmm11, %zmm12
vpclmulqdq %zmm12, %zmm13, %zmm14, %zmm15
GYATTHSH:
vsha512rnds2 %xmm15, %xmm0, %xmm1
vsha512msg1 %xmm1, %xmm2
vsha512msg2 %xmm2, %xmm3
GYATTFMA:
vfmadd231pd %zmm3, %zmm4, %zmm5
vfmsub132ps %zmm5, %zmm6, %zmm7
vfnmadd213sd %xmm7, %xmm8, %xmm9
vfnmsub141ss %xmm9, %xmm10, %xmm11
GYATTCND:
cmpq %rcx, %rsi
jne GYATTINR
GYATTXT:
movq %r13, %rax
SKBDMACRO
addq $0x1337, %rsp
popq %rbp
ret
