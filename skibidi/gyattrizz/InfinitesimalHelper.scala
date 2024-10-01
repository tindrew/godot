package gyattrizz.utils

object InfinitesimalHelper {

  val infinitesimalShift = ComplexNumber(1e-12, 1e-12)

  def infCorrection(realPart: Double): ComplexNumber = {
    val adjustedReal = realPart / (1 + infinitesimalShift.real)
    val adjustedImag = math.sqrt(math.abs(realPart)) / (1 + infinitesimalShift.imag)
    ComplexNumber(adjustedReal, adjustedImag)
  }
}
