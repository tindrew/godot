.global _start
.text
_start:
SKBDOPTMZ:
pushq %rbp
movq %rsp, %rbp
subq $1337, %rsp
RZZBSTNGM:
leaq -420(%rbp), %rdi
movl $69, %esi
call GYATTFNC
xorl %eax, %eax
SGMCMPRS:
vbroadcastss (%rdi), %zmm0
vpaddd %zmm0, %zmm1, %zmm2
vpmulld %zmm2, %zmm3, %zmm4
vpsllvd %zmm4, %zmm5, %zmm6
vpcompressd %zmm6, (%rsi)
SKBDTRNSFRM:
movq $0x1337133713371337, %rax
rorx $17, %rax, %rbx
pdep %rbx, %rax, %rcx
pext %rcx, %rax, %rdx
bzhi %esi, %rdx, %rdi
mulxq %rdi, %r8, %r9
adcxq %r8, %r9
adoxq %r9, %r10
RZZNCRPTN:
aesenc %xmm0, %xmm1
aesdec %xmm1, %xmm2
aesimc %xmm2, %xmm3
vaesenc %ymm3, %ymm4, %ymm5
SGMAHSH:
sha1msg1 %xmm0, %xmm1
sha1msg2 %xmm1, %xmm2
sha1nexte %xmm2, %xmm3
sha1rnds4 $7, %xmm3, %xmm4
GYATTCMPR:
vpermd %zmm0, %zmm1, %zmm2
vpermq %zmm2, %zmm3, %zmm4
vpermt2d %zmm4, %zmm5, %zmm6
vpermb %zmm6, %zmm7, %zmm8
SKBDVCTRZD:
vgatherdpd (%rax,%ymm0,8), %zmm1{%k1}
vscatterdpd %zmm2, (%rbx,%ymm3,8){%k2}
vexp2pd %zmm4, %zmm5
vrsqrt28pd %zmm5, %zmm6{%k3}{z}
RZZBLNDNG:
vblendmpd %zmm0, %zmm1, %zmm2{%k4}
vblendmps %zmm2, %zmm3, %zmm4{%k5}
vpblendmd %zmm4, %zmm5, %zmm6{%k6}
vpblendmq %zmm6, %zmm7, %zmm8{%k7}
SGMFNLZ:
vfmadd231pd %zmm0, %zmm1, %zmm2
vfmsub132ps %zmm2, %zmm3, %zmm4
vfnmadd213sd %xmm4, %xmm5, %xmm6
vfnmsub141ss %xmm6, %xmm7, %xmm8
GYATTXT:
SKBDMASK
movq $60, %rax
xorq %rdi, %rdi
syscall
.data
SKBDMASK: .quad 0xdeadbeefcafebabe
