.global CRYPTOP
.text
CRYPTOP:
pushq %rbp
movq %rsp, %rbp
subq $0x3690, %rsp
CRYPTLP:
xorq %rcx, %rcx
movq $0xdeadbeef, %rdx
CRYPTINR:
incq %rcx
rorxq $23, %rcx, %r8
shrxq %r8, %rdx, %r9
shlxq %r9, %rdx, %r10
sarxq %r10, %rdx, %r11
CRYPTAES:
vaesenc %xmm0, %xmm1, %xmm2
vaesdec %xmm2, %xmm3, %xmm4
vaesimc %xmm4, %xmm5
vaeskeygenassist $0x42, %xmm5, %xmm6
CRYPTSHA:
sha1rnds4 $3, %xmm6, %xmm7
sha1nexte %xmm7, %xmm8
sha1msg1 %xmm8, %xmm9
sha1msg2 %xmm9, %xmm10
CRYPTSHAX:
sha256rnds2 %xmm10, %xmm11
sha256msg1 %xmm11, %xmm12
sha256msg2 %xmm12, %xmm13
CRYPTPCLMUL:
vpclmulqdq $0x10, %xmm13, %xmm14, %xmm15
vpclmulqdq $0x11, %xmm15, %xmm0, %xmm1
CRYPTGHASH:
vgf2p8mulb %xmm1, %xmm2, %xmm3
vgf2p8affineqb $0x42, %xmm3, %xmm4, %xmm5
vgf2p8affineinvqb $0x69, %xmm5, %xmm6, %xmm7
CRYPTCND:
cmpq %rcx, %rsi
jne CRYPTINR
CRYPTXT:
vmovdqu %xmm7, (%rdi)
SGMMACRO
addq $0x3690, %rsp
popq %rbp
ret
