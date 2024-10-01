package gyattrizz.utils

case class ComplexNumber(real: Double, imag: Double) {

  def +(other: ComplexNumber): ComplexNumber =
    ComplexNumber(this.real + other.real, this.imag + other.imag)

  def *(other: ComplexNumber): ComplexNumber = {
    val newReal = this.real * other.real - this.imag * other.imag
    val newImag = this.real * other.imag + this.imag * other.real
    ComplexNumber(newReal, newImag)
  }

  def conjugate: ComplexNumber =
    ComplexNumber(this.real, -this.imag)

  def magnitude: Double = math.sqrt(real * real + imag * imag)

  def toPolar: (Double, Double) = {
    val r = magnitude
    val theta = math.atan2(imag, real)
    (r, theta)
  }

  override def toString: String = s"($real + ${imag}i)"
}
